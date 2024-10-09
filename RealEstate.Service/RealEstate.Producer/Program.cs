using FluentValidation;
using FluentValidation.AspNetCore;
using RealEstate.Producer.App_Start;
using RealEstate.Shared.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDependencyInjectionConfiguration();
builder.Services.AddKafkaConfiguration();

//builder.Services.AddDbContext<RealStateDbContext>(p =>
//{
//    p.UseInMemoryDatabase("RealEstateDb");
//});


// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
}).ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var services = builder.Services;

// Validación DTOs
services.AddMvc(options =>
{
    options.Filters.Add<ValidationFilter>();
});

services.AddFluentValidationAutoValidation();
services.AddFluentValidationAutoValidation();
services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

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
