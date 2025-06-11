using Interasian.API.Data;
using Interasian.API.MappingProfiles;
using Interasian.API.Repositories;
using Interasian.API.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Interasian.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Mapper));

// Repositories
builder.Services.AddScoped<IListingRepository, ListingRepository>();
builder.Services.AddScoped<IListingImageRepository, ListingImageRepository>();
//builder.Services.AddScoped<IAuthRepository, AuthRepository>();

// Services
builder.Services.AddScoped<IEmailService, EmailService>();

// Use the Serilog.AspNetCore NuGet Package
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

// Use the Serilog.AspNetCore NuGet Package
builder.Host.UseSerilog();

// Use Dbb Context
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
});


// Configure Identity
//builder.Services.AddIdentity<Use>()
//	.AddEntityFrameworkStores<DatabaseContext>() 
//	.AddDefaultTokenProviders();

// Configure JWT Authentication
//var jwtSettings = builder.Configuration.GetSection("JwtConfig");
//var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key not found"));

//builder.Services.AddAuthentication(options =>
//{
//	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//	options.TokenValidationParameters = new TokenValidationParameters
//	{
//		ValidateIssuer = true,
//		ValidateAudience = true,
//		ValidateLifetime = true,
//		ValidateIssuerSigningKey = true,
//		ValidIssuer = jwtSettings["Issuer"],
//		ValidAudience = jwtSettings["Audience"],
//		IssuerSigningKey = new SymmetricSecurityKey(key)
//	};
//});

builder.Services.AddAuthorization();

var app = builder.Build();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
