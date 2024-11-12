using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using tStudy.Application.Interfaces;
using tStudy.Application.Services;
using tStudy.Models.Data;
using tStudy.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

// Thêm d?ch v? Authentication v?i JWT Bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],   // ??a ch? Issuer
            ValidAudience = builder.Configuration["Jwt:Audience"], // ??a ch? Audience
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])) // Secret key
        };
    });

builder.Services.AddDbContext<tStudyDbContext>( options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("tStudy")));

builder.Services.AddIdentity<SystemUser, IdentityRole>()
            .AddEntityFrameworkStores<tStudyDbContext>()
            .AddDefaultTokenProviders();

// ??ng ký AuthService
builder.Services.AddScoped<IAuthService, AuthService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
