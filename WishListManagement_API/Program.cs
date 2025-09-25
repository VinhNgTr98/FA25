using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WishListManagement_API.Data;
using WishListManagement_API.Repositories;
using WishListManagement_API.Services;
var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<WishListManagement_APIContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("WishListManagement_APIContext") ?? throw new InvalidOperationException("Connection string 'WishListManagement_APIContext' not found.")));
builder.Services.AddDbContext<WishListManagement_APIContext>(options =>
options.UseMySql(
builder.Configuration.GetConnectionString("DefaultConnection"),
new MySqlServerVersion(new Version(8, 0, 36)) // đổi đúng version MySQL trên Railway
)
);
// Add services to the container.
    builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
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
