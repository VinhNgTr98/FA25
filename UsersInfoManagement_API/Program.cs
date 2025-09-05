using Microsoft.EntityFrameworkCore;
using UsersInfoManagement_API.Data;
using UsersInfoManagement_API.Repositories;
using UsersInfoManagement_API.Services;
var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<UsersInfoManagement_APIContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("UsersInfoManagement_APIContext") ?? throw new InvalidOperationException("Connection string 'UsersInfoManagement_APIContext' not found.")));
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

builder.Services.AddDbContext<UsersInfoManagement_APIContext>(options =>
options.UseMySql(
builder.Configuration.GetConnectionString("DefaultConnection"),
new MySqlServerVersion(new Version(8, 0, 36)) // đổi đúng version MySQL trên Railway
)
);
// Add services to the container.



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Repositories
builder.Services.AddScoped<IUsersInfoRepository, UsersInfoRepository>();

// Services
builder.Services.AddScoped<IUsersInfoService, UsersInfoService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();

// bật CORS (đặt trước Authentication/Authorization)
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
