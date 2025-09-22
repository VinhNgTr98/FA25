using CarManagement_API.Data;
using CarManagement_API.DTOs;
using CarManagement_API.Models;
using CarManagement_API.Profiles;
using CarManagement_API.Repositories;
using CarManagement_API.Repositories.Interfaces;
using CarManagement_API.Services;
using CarManagement_API.Services.Interfaces;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

// ===== CORS =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// DbContext (MySQL)
builder.Services.AddDbContext<CarManagement_APIContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36))
));

// ===== OData + Controllers =====
var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EntitySet<Car>("Cars"); // Sử dụng entity Car thay vì CarReadDto

builder.Services.AddControllers().AddOData(options => options
    .AddRouteComponents("odata", odataBuilder.GetEdmModel()) // Đảm bảo sử dụng EDM Model đúng
    .SetMaxTop(100)
    .Count()
    .Filter()
    .OrderBy()
    .Expand()
    .Select()
);

// Thêm AutoMapper
builder.Services.AddAutoMapper(typeof(CarProfile).Assembly);

// Thêm Repository và Service cho Car
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICarService, CarService>();

builder.Services.AddControllers();

// Swagger/OpenAPI Configuration
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

// Bật CORS (đặt trước Authentication/Authorization)
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
