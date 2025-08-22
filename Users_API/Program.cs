using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User_API.Models;
using User_API.Profiles;
using User_API.Repositories;
using Users_API.Data;
using Users_API.Services;
var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<Users_APIContext>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("Users_APIContext") ?? throw new InvalidOperationException("Connection string 'Users_APIContext' not found.")));
builder.Services.AddDbContext<Users_APIContext>(options =>
options.UseMySql(
builder.Configuration.GetConnectionString("DefaultConnection"),
new MySqlServerVersion(new Version(8, 0, 36)) // đổi đúng version MySQL trên Railway
)
);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

app.UseAuthorization();

app.MapControllers();

app.Run();
