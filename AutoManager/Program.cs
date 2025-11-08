using AutoManager.AutoManager_Application.Services;
using AutoManager.AutoManager_Domain.Interfaces;
using AutoManager.AutoManager_Infrastructure.Config;
using AutoManager.AutoManager_Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
     .AddJsonOptions(options =>
     {
         // IMPORTANTE: Ignora ciclos de referencia circulares
         options.JsonSerializerOptions.ReferenceHandler =
             System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

         options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
         options.JsonSerializerOptions.DefaultIgnoreCondition =
             System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
         options.JsonSerializerOptions.NumberHandling =
             System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
     });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConexionPg"))
);

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();

builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<VehicleService>();
builder.Services.AddScoped<MaintenanceService>();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();