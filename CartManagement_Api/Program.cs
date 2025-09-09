using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CartManagement_Api.Data;
using CartManagement_Api.Repositories;
using CartManagement_Api.Services;
var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<CartManagement_ApiContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("CartManagement_ApiContext") ?? throw new InvalidOperationException("Connection string 'CartManagement_ApiContext' not found.")));

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
builder.Services.AddDbContext<CartManagement_ApiContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36))
));
// Add services to the container.
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
app.UseAuthorization();

app.MapControllers();

app.Run();
