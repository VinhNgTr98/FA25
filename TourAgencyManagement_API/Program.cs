using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using TourAgencyManagement_API.Data;
using TourAgencyManagement_API.DTOs;
using TourAgencyManagement_API.Repositories;
using TourAgencyManagement_API.Repositories.Interfaces;
using TourAgencyManagement_API.Services;
using TourAgencyManagement_API.Services.Interfaces;
using AutoMapper; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddOData(options => options
        .AddRouteComponents("odata", new ODataConventionModelBuilder().GetEdmModel())
        .SetMaxTop(100)
        .Count()
        .Filter()
        .OrderBy()
        .Expand()
        .Select());

builder.Services.AddDbContext<TourAgencyContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36))
    )
);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TourAgencyManagementAPI", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập token theo dạng: Bearer {your JWT token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// ĐĂNG KÝ AUTOMAPPER THỦ CÔNG (tạm bỏ AddAutoMapper để tránh xung đột)
var mapperConfig = new MapperConfiguration(cfg =>
{
    // Quét tất cả profiles trong assembly của bạn
    cfg.AddMaps(typeof(TourAgencyManagement_API.Profiles.TourAgencyProfile).Assembly);
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// DI khác
builder.Services.AddScoped<ITourAgencyRepository, TourAgencyRepository>();
builder.Services.AddScoped<ITourAgencyService, TourAgencyService>();


// ===== CORS =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});
var app = builder.Build();

app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();