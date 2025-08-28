using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using User_API.Models;
using User_API.Profiles;
using User_API.DTOs;
using User_API.Repositories;
using Users_API.Data;
using Users_API.Services;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.OData;
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

//swagger authen
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserManagementAPI", Version = "v1" });

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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();
var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EntitySet<UserReadDto>("Users");
// Add services to the container.

builder.Services.AddControllers().AddOData(options => options
  .AddRouteComponents("odata", odataBuilder.GetEdmModel())
  .SetMaxTop(100)
   .Count()
   .Filter()
   .OrderBy()
   .Expand()
   .Select());

var app = builder.Build();


// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
