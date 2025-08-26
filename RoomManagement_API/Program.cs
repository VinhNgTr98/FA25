using Microsoft.EntityFrameworkCore;
using RoomManagement_API.Data;
using RoomManagement_API.Repositories.Rooms;
using RoomManagement_API.Services.Rooms;
var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<RoomManagement_APIContext>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("RoomManagement_APIContext") ?? throw new InvalidOperationException("Connection string 'RoomManagement_APIContext' not found.")));
builder.Services.AddDbContext<RoomManagement_APIContext>(options =>
options.UseMySql(
builder.Configuration.GetConnectionString("DefaultConnection"),
new MySqlServerVersion(new Version(8, 0, 36)) // đổi đúng version MySQL trên Railway
)
);
// Add services to the container.
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomService, RoomService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
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
