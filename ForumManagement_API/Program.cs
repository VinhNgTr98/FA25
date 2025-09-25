using ForumPostManagement_API.Data;
using ForumPostManagement_API.Mapping;
using ForumPostManagement_API.Repositories;
using ForumPostManagement_API.Services;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure; // ServerVersion.AutoDetect

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

// ===== Database: MySQL (Pomelo) =====
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

var serverVersion = ServerVersion.AutoDetect(connectionString);

builder.Services.AddDbContext<ForumPostManagement_APIContext>(options =>
    options.UseMySql(
        connectionString,
        serverVersion,
        my => my.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null))
);

// ===== AutoMapper + Repository + Service =====
builder.Services.AddAutoMapper(typeof(ForumPostProfile).Assembly);
builder.Services.AddScoped<IForumPostRepository, ForumPostRepository>();
builder.Services.AddScoped<IForumPostService, ForumPostService>();

// ===== Controllers + Swagger =====
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ===== Swagger (bật ở mọi môi trường để dễ test) =====
app.UseSwagger();
app.UseSwaggerUI();

// ===== Pipeline =====
app.UseHttpsRedirection();

// Bật CORS trước AuthZ
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Tránh 404 khi vào "/" -> chuyển sang Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();