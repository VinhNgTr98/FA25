using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Categories_API.Data;
using Categories_API.Repositories;
using Categories_API.Services;
var builder = WebApplication.CreateBuilder(args);
/*builder.Services.AddDbContext<Categories_APIContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Categories_APIContext") ?? throw new InvalidOperationException("Connection string 'Categories_APIContext' not found.")));*/
builder.Services.AddDbContext<Categories_APIContext>(options =>
options.UseMySql(
builder.Configuration.GetConnectionString("DefaultConnection"),
new MySqlServerVersion(new Version(8, 0, 36)) // đổi đúng version MySQL trên Railway
)
);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();


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
