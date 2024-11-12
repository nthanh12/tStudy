using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using tStudy.Application.Interfaces;
using tStudy.Application.Services;
using tStudy.Models.Data;
using tStudy.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

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
