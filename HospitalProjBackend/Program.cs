//var builder = WebApplication.CreateBuilder(args);

//Add services to the container.

//builder.Services.AddControllers();
//Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

//var app = builder.Build();

//Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
using HospitalProjBackend.Data;
using Swashbuckle.AspNetCore;
using HospitalProjBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// load env vars & user secrets
builder.Configuration.AddEnvironmentVariables();

// connection string (env var overrides appsettings)
var conn = builder.Configuration.GetConnectionString("DefaultConnection")
           ?? builder.Configuration["ConnectionStrings:DefaultConnection"]
           ?? "Server=.;Database=HospitalAppointmentsDb;Trusted_Connection=True;";

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// DI services
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ISmsService, SmsService>();
builder.Services.AddSingleton<JwtService>();
builder.Services.AddScoped<IPasswordHasher<HospitalProjBackend.Models.User>, PasswordHasher<HospitalProjBackend.Models.User>>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT config - MUST set Jwt__Key in environment for production
var jwtKey = builder.Configuration["Jwt__Key"] ?? builder.Configuration["Jwt:Key"] ?? throw new Exception("JWT key not configured");
var issuer = builder.Configuration["Jwt__Issuer"] ?? builder.Configuration["Jwt:Issuer"] ?? "hospital.local";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// CORS: allow frontend url from env or default
builder.Services.AddCors(options =>
    options.AddPolicy("Frontend",
        p => p.WithOrigins(builder.Configuration["Frontend__Url"] ?? "http://localhost:4200")
              .AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
