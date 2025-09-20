using Microsoft.EntityFrameworkCore;
using BookingManagement_API.Data;
using BookingManagement_API.Repositories;
using BookingManagement_API.Services;

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

// Add services to the container.
builder.Services.AddDbContext<BookingManagement_APIContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36))
    )
);

// Đăng ký Repository và Service cho Booking
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddControllers();
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