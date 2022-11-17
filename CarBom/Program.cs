using CarBom.Mappers;
using CarBom.Requests;
using CarBom.Validators;
using DataProvider;
using DataProvider.DataModels;
using DataProvider.Repositories;
using FluentValidation;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CarBom API",
        Version = "v1"
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddSingleton<DBContext>();
builder.Services.AddScoped<IMechanicRepository, MechanicRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IOrderedServiceRepository, OrderedServiceRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IMechanicMapper, MechanicMapper>();
builder.Services.AddScoped<IOrderedServiceMapper, OrderedServiceMapper>();
builder.Services.AddScoped<IErrorResponseMapper, ErrorResponseMapper>();
builder.Services.AddSingleton<IValidator<MechanicRequest>, MechanicRequestValidator>();
builder.Services.AddSingleton<IValidator<OrderedServiceRequest>, OrderedServiceRequestValidator>();
builder.Services.AddSingleton<IValidator<ServiceRequest>, ServiceRequestValidator>();
builder.Services.AddSingleton<IValidator<UserRequest>, UserRequestValidator>();
builder.Services.AddSingleton<IValidator<User>, UserValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
