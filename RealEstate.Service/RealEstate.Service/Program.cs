using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RealEstate.Database;
using RealEstate.Service;

Console.WriteLine("Start consuming events ...");

var builder = Host.CreateApplicationBuilder();

builder.Services.AddDbContext<ReportDbContext>(p =>
{
    p.UseSqlServer("server=DESKTOP-S6GPPIB;Database=dbRealEstateKafka;User Id=sa;Password=123456789;TrustServerCertificate=True;MultipleActiveResultSets=True");
}, ServiceLifetime.Singleton);

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddHostedService<Worker>();

builder.Build().Run();