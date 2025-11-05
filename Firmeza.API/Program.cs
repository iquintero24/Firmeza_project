using DotNetEnv;
using Firmeza.web.Data;
using Microsoft.EntityFrameworkCore;

// load of env
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// services configuration 
// 1.0 CORS: configuration of political for access clients external 

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", 
        builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

//1.1 add controllers 
builder.Services.AddControllers();

// Database (Task2:) : connection 
var connectionString = Environment.GetEnvironmentVariable("SUPABASE_CONNECTION_STRING");

if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetValue<string>("SUPABASE_CONNECTION_STRING");
}

if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Connection string not found:  SUPABASE_CONNECTION_STRING (Check your .env or appsettings)");
}

// configuration of Dbcontext for npgsql (driver)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount:5,
            maxRetryDelay:TimeSpan.FromSeconds(30),
            errorCodesToAdd:null);
    }));


// Repositories & Services Injection (Task: 2): inject logic business


//1.2 Add swagger/OpenAi (documentation)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//2. HTTP REQUEST PIPELINE
var app = builder.Build();

// configure the HTTP request pipeline 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // Configure for the swagger the pag index (/)
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json","Firmeza API V1");
        c.RoutePrefix = string.Empty;
    });
} 

app.UseHttpsRedirection();

// 2.1 maps controllers 
app.MapControllers();

app.Run();