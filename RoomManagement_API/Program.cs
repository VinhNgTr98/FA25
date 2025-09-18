using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using RoomManagement_API.Data;
using RoomManagement_API.DTOs;
using RoomManagement_API.Models;
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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RoomManagementAPI", Version = "v1" });

    // Cấu hình hỗ trợ Bearer token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập token theo dạng: Bearer {your JWT token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
});



var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EntitySet<Room>("RoomManagementAPI");
// Add services to the container.

builder.Services.AddControllers().AddOData(options => options
  .AddRouteComponents("odata", odataBuilder.GetEdmModel())
  .SetMaxTop(100)
   .Count()
   .Filter()
   .OrderBy()
   .Expand()
   .Select());
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
var app = builder.Build();

app.UseCors("AllowAll");
// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
