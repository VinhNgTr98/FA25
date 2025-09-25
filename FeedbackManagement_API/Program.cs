using FeedbackManagement_API.Data;
using FeedbackManagement_API.Profiles;
using FeedbackManagement_API.Repositories;
using FeedbackManagement_API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<FeedbackManagement_APIContext>(options =>
options.UseMySql(
builder.Configuration.GetConnectionString("DefaultConnection"),
new MySqlServerVersion(new Version(8, 0, 36)) // đổi đúng version MySQL trên Railway
)
);


// AutoMapper (scan đúng assembly chứa FeedbackProfile)
builder.Services.AddAutoMapper(typeof(FeedbackProfile).Assembly);

// Đăng ký Repository và Service
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

app.UseAuthorization();

app.MapControllers();

app.Run();
