using Interasian.API.Data;
using Interasian.API.MappingProfiles;
using Interasian.API.Repositories;
using Interasian.API.Services;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Interasian.API.Models;
using Interasian.API.Utilities;
using AspNetCoreRateLimit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Mapper));

// MongoDB Context
builder.Services.AddSingleton<MongoDbContext>();

// Repositories
builder.Services.AddScoped<IListingRepository, ListingRepository>();

// Services
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddSingleton<DataMigrationService>();

// Utils
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

// Use the Serilog.AspNetCore NuGet Package
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

// Use the Serilog.AspNetCore NuGet Package
builder.Host.UseSerilog();

// Caching
builder.Services.AddMemoryCache();

// Rate Limiting
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.GeneralRules =
    [
        new RateLimitRule
        {
            Endpoint = "*",
            Period = "1m",
            Limit = 100
        }
    ];

    options.QuotaExceededResponse = new QuotaExceededResponse
    {
        Content = "Too many requests. Please try agian later",
        ContentType = "application/json",
        StatusCode = StatusCodes.Status429TooManyRequests
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Migrate data on startup
var migrationService = app.Services.GetRequiredService<DataMigrationService>();
await migrationService.MigrateDataAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use the Serilog.AspNetCore NuGet Package
app.UseSerilogRequestLogging();

app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseHttpsRedirection();

app.UseIpRateLimiting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
