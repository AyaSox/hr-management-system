using HRManagementSystem.Data;
using HRManagementSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using System.Globalization;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Localization; // added

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en-ZA", "en-US" };
    options.SetDefaultCulture("en-ZA")
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

// Caching
builder.Services.AddMemoryCache();

// Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HR Management System API",
        Version = "v1",
        Description = "API endpoints for HR Management System"
    });
});

// Configure connection string - use SQLite for persistent storage
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=hrmanagement.db";

// Use SQLite database for persistent storage
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString.Replace("hrmanagement.db", "identity.db")));

// Hangfire with in-memory storage for demo
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseInMemoryStorage());

builder.Services.AddHangfireServer();

// Configure Identity with optimized settings
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;
    
    // Optimize lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Register services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuditService>();
builder.Services.AddScoped<IBackgroundJobTasks, BackgroundJobTasks>();

// Background reminders hosted service
builder.Services.AddHostedService<RemindersHostedService>();

// Configure QuestPDF license
QuestPDF.Settings.License = LicenseType.Community;

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HR Management System API v1");
    });
}

// Hangfire Dashboard (Admin only)
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new IDashboardAuthorizationFilter[] { } // Remove authorization for testing
});

// OR for production, use this instead:
// app.UseHangfireDashboard("/hangfire", new DashboardOptions
// {
//     Authorization = new[] { new HangfireAuthorizationFilter() }
// });

// Configure South African culture for currency formatting by default
var defaultCulture = new CultureInfo("en-ZA");
defaultCulture.NumberFormat.CurrencyGroupSeparator = " ";
defaultCulture.NumberFormat.CurrencyDecimalSeparator = ".";
defaultCulture.NumberFormat.CurrencySymbol = "R";
CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

// Apply localization to current request with supported cultures
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-ZA"),
    SupportedCultures = new[] { new CultureInfo("en-ZA"), new CultureInfo("en-US") },
    SupportedUICultures = new[] { new CultureInfo("en-ZA"), new CultureInfo("en-US") }
};
app.UseRequestLocalization(localizationOptions);

// Lightweight endpoint to set culture cookie and persist selection
app.MapGet("/set-culture", (string culture, string? returnUrl, HttpContext httpContext) =>
{
    var supported = new[] { "en-ZA", "en-US" };
    if (!supported.Contains(culture)) culture = "en-ZA"; // fallback

    httpContext.Response.Cookies.Append(
        CookieRequestCultureProvider.DefaultCookieName,
        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), HttpOnly = false, IsEssential = true }
    );

    return Results.Redirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Background seeding after the server has started listening
app.Lifetime.ApplicationStarted.Register(() =>
{
    _ = Task.Run(async () =>
    {
        await Task.Delay(1000);
        try
        {
            // Update database schema first (temporary safety for existing DBs without full migrations)
            DatabaseUpdater.AddIsDeletedColumn();

            using var scope = app.Services.CreateScope();
            var appContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var identityContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var jobs = scope.ServiceProvider.GetRequiredService<IBackgroundJobTasks>();

            // Apply migrations to ensure database schema is up to date for the application DB
            await appContext.Database.MigrateAsync();

            // Identity DB now also uses migrations
            await identityContext.Database.MigrateAsync();

            await RoleSeeder.SeedAsync(scope.ServiceProvider);
            await DemoDataSeeder.SeedAsync(appContext);

            // Schedule recurring background jobs
            RecurringJob.AddOrUpdate("birthday-reminders",
                () => jobs.SendBirthdayReminders(), Cron.Daily(6));

            RecurringJob.AddOrUpdate("anniversary-reminders",
                () => jobs.SendAnniversaryReminders(), Cron.Daily(6));

            RecurringJob.AddOrUpdate("monthly-headcount-report",
                () => jobs.GenerateMonthlyHeadcountReport(), Cron.Monthly(1, 7)); // 1st of month 07:00

            RecurringJob.AddOrUpdate("daily-salary-band-report",
                () => jobs.GenerateSalaryBandReport(), Cron.Daily(19)); // daily at 19:00

            Console.WriteLine("? Background jobs scheduled. Visit /hangfire to view.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"? Background seeding error: {ex.Message}");
        }
    });
});

await app.RunAsync();

// Hangfire authorization filter
public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        return httpContext.User.IsInRole("Admin");
    }
}
