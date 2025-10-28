
using DotNetEnv;
using Firmeza.web.Data;
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
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

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