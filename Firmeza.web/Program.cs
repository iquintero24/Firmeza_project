
using DotNetEnv;
using Firmeza.web.Data;
using Firmeza.web.Data.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// load the environment variables:
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// get the connection string 
var connectionString = builder.Configuration.GetValue<string>("SUPABASE_CONNECTION_STRING");

// (*) using the direct connection for migrations:

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" && args.Contains("database update"))
{
    connectionString = builder.Configuration.GetValue<string>("SUPABASE_MIGRATION_STRING");
}

// (*) conditional if the connection string is not found:
if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Connection string not found: SUPABASE_CONNECTION_STRING");
}

// (*) configuration of the dbContext for use Npgsql (Driver) and the connection pooler;
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount:5,
            maxRetryDelay:TimeSpan.FromSeconds(30),
            errorCodesToAdd:null);
    }));

// (*) Configuration identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // configurations for passwords
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 6;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// configuration redirect for access denied
builder.Services.ConfigureApplicationCookie(options =>
{
    // redirect to user if not have rol admin
    options.AccessDeniedPath = "/Home/AccessDenied";

    // redirect to login page if authentication is required
    options.LoginPath = "/Identity/Account/Login";
});

// register politic auth
builder.Services.AddAuthorization(options =>
{
    // define the politic "AdminPolicy" that required the rol Administrator 
    options.AddPolicy("AdminPolicy",policy =>
        policy.RequireRole("Administrator"));
});


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed roles and admin user
using (var scope = app.Services.CreateScope())
{
    var servicesProvider = scope.ServiceProvider;
    try
    {
        // call the method to seed roles and admin user
        await DataSeeder.SeedRolesAndAdminUserAsync(servicesProvider);
    }
    catch (Exception e)
    {
        // log the error if something goes wrong during seeding
        var logger = servicesProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(e, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();