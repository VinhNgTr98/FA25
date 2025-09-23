using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using MotorbikeManageMent_API.Data;
using MotorbikeManageMent_API.DTOs;
using MotorbikeManageMent_API.Repositories;
using MotorbikeManageMent_API.Repositories.Interfaces;
using MotorbikeManageMent_API.Services;
using MotorbikeManageMent_API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Database Configuration (MySQL)
builder.Services.AddDbContext<MotorbikeContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")),
        my => my.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null)
    )
);

// Add Controllers
builder.Services.AddControllers();

// Add Authorization and Authentication (necessary for Bearer tokens)
//builder.Services.AddAuthentication("Bearer")
//    .AddJwtBearer(options =>
//    {
//        options.Authority = builder.Configuration["Authentication:Authority"]; // Your Auth server
//        options.Audience = builder.Configuration["Authentication:Audience"];
//        options.RequireHttpsMetadata = false;
//    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

// Swagger Configuration for API Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "VehicleManagementAPI", Version = "v1" });

    // Configure Swagger to support Bearer token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter token as: Bearer {your JWT token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// AutoMapper Configuration
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Register services
builder.Services.AddScoped<IMotorbikeRepository, MotorbikeRepository>();
builder.Services.AddScoped<IMotorbikeService, MotorbikeService>();

// OData Configuration
var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EntitySet<MotorbikeReadDto>("Vehicles");

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline

app.UseCors("AllowAll");

// Enable Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

// Enable Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// Enable HTTPS Redirection
app.UseHttpsRedirection();

// Enable Authorization
app.MapControllers();

app.Run();
