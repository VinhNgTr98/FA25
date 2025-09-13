using CartManagement_Api.Data;
using CartManagement_Api.DTOs;
using CartManagement_Api.Profiles;
using CartManagement_Api.Repositories;
using CartManagement_Api.Repositories.Interfaces;
using CartManagement_Api.Services;
using CartManagement_Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// DbContext MySQL
builder.Services.AddDbContext<CartManagement_ApiContext>(opt =>
{
    opt.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")));
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(CartProfile));

// DI
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CartManagementAPI", Version = "v1" });

    // Cấu hình hỗ trợ Bearer token
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

var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EntitySet<CartReadDto>("Cart");
// Add services to the container.

builder.Services.AddControllers().AddOData(options => options
  .AddRouteComponents("odata", odataBuilder.GetEdmModel())
  .SetMaxTop(100)
   .Count()
   .Filter()
   .OrderBy()
   .Expand()
   .Select());
var app = builder.Build();


app.UseCors("AllowAll");


    app.UseSwagger();
    app.UseSwaggerUI();


app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();


app.Run();