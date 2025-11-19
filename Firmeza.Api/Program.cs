using System.Text;
using Application.Mappings;
using DotNetEnv;
using Firmeza.Application.Implemetations;
using Firmeza.Application.Interfaces;
using Firmeza.Domain.Entities;
using Firmeza.Domain.Interfaces;
using Firmeza.Infrastructure.Persistence;
using Firmeza.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// ============================================
// 🔹 DATABASE CONFIGURATION
// ============================================
var connectionString = builder.Configuration.GetValue<string>("SUPABASE_CONNECTION_STRING");

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" && args.Contains("database update"))
{
    connectionString = builder.Configuration.GetValue<string>("SUPABASE_MIGRATION_STRING");
}

if (string.IsNullOrEmpty(connectionString))
    throw new Exception("Connection string not found: SUPABASE_CONNECTION_STRING");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorCodesToAdd: null);
    }));

// ============================================
// 🔹 IDENTITY CONFIGURATION
// ============================================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ============================================
// 🔹 JWT CONFIGURATION
// ============================================
var jwtKey = Environment.GetEnvironmentVariable("TOKEN_JWT");
if (string.IsNullOrEmpty(jwtKey))
    throw new Exception("TOKEN_JWT not configured in .env");

var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

// ============================================
// 🔹 DEPENDENCY INJECTION
// ============================================
builder.Services.AddScoped<ITokenService, TokenService>();
// register application services
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>(); 
builder.Services.AddScoped<ICustomerService, CustomerService>();       

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ISaleService, SaleService>();


builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// ============================================
// 🔹 CONTROLLERS
// ============================================
builder.Services.AddControllers(); // 👈 FALTA ESTO para que Swagger detecte los controladores

// ============================================
// 🔹 SWAGGER CONFIGURATION
// ============================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Firmeza API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Insert the JWT token preceded by 'Bearer '",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

// ============================================
// 🔹 BUILD APPLICATION
// ============================================
var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("ClientPolicy");

// 👇 ESTA LÍNEA MAPEARÁ TODOS TUS CONTROLADORES
app.MapControllers();

app.Run();
