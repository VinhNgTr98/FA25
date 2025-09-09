using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement_API.Data;
using OrderManagement_API.Repositories;
using OrderManagement_API.Services;
var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<OrderManagement_APIContext>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("OrderManagement_APIContext") ?? throw new InvalidOperationException("Connection string 'OrderManagement_APIContext' not found.")));

builder.Services.AddDbContext<OrderManagement_APIContext>(options =>
   options.UseMySql(
       builder.Configuration.GetConnectionString("DefaultConnection"),
       new MySqlServerVersion(new Version(8, 0, 36))
   )
);


builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
