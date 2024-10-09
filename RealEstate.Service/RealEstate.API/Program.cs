using Microsoft.EntityFrameworkCore;
using RealEstate.API.App_Start;
using RealEstate.Database;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add database context
var conn = SwitchConnectionConfiguration.GetConnection();
builder.Services.AddDbContext<ReportDbContext>(opt => opt.UseSqlServer(conn, p => p.MigrationsAssembly("RealEstate.Database")));

builder.Services.AddDependencyInjectionConfiguration();
builder.Services.AddOptions(builder.Configuration);

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
