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

// Configure connection string - use in-memory for demo if needed
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Use in-memory database for Azure demo to avoid startup timeouts
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("HRManagementDB"));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("IdentityDB"));

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

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Start the app immediately - do seeding in background after startup
app.Start();

// Background seeding after startup to avoid timeout
_ = Task.Run(async () =>
{
    await Task.Delay(2000); // Wait 2 seconds for app to be ready
    
    try
    {
        using var scope = app.Services.CreateScope();
        
        var appContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var identityContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        // Ensure databases are created
        await appContext.Database.EnsureCreatedAsync();
        await identityContext.Database.EnsureCreatedAsync();
        
        // Seed roles and admin user
        await RoleSeeder.SeedAsync(scope.ServiceProvider);
        
        // Seed demo data
        await DemoDataSeeder.SeedAsync(appContext);
        
        Console.WriteLine("? Background seeding completed successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"? Background seeding error: {ex.Message}");
    }
});

await app.WaitForShutdownAsync();
