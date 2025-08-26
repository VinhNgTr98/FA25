using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement_API.Data;
using OrderManagement_API.Profil;
using OrderManagement_API.Repositories;
using OrderManagement_API.Services;
var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<OrderManagement_APIContext>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("OrderManagement_APIContext") ?? throw new InvalidOperationException("Connection string 'OrderManagement_APIContext' not found.")));
builder.Services.AddDbContext<OrderManagement_APIContext>(options =>
options.UseMySql(
builder.Configuration.GetConnectionString("DefaultConnection"),
new MySqlServerVersion(new Version(8, 0, 36)) // đổi đúng version MySQL trên Railway
)
);
// Add services to the container.
builder.Services.AddAutoMapper(typeof(OrderProfile).Assembly);
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
