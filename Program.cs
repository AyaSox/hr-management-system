using HRManagementSystem.Data;
using HRManagementSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Configure connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Domain DB with optimized settings
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.CommandTimeout(30); // Set timeout
        sqlOptions.EnableRetryOnFailure(); // Auto retry on failure
    });
    
    // Only enable sensitive data logging in development
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
    }
});

// Identity DB with same optimizations
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.CommandTimeout(30);
        sqlOptions.EnableRetryOnFailure();
    });
});

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

// Configure QuestPDF license
QuestPDF.Settings.License = LicenseType.Community;

var app = builder.Build();

// Configure South African culture for currency formatting
var cultureInfo = new CultureInfo("en-ZA");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages(); // Identity UI
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Optimize role seeding - run in background
_ = Task.Run(async () =>
{
    try
    {
        using var scope = app.Services.CreateScope();
        await RoleSeeder.SeedAsync(scope.ServiceProvider);
        
        // Seed demo data if in development
        if (app.Environment.IsDevelopment())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await DemoDataSeeder.SeedAsync(context);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Background seeding error: {ex.Message}");
    }
});

app.Run();
