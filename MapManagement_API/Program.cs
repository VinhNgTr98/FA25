var builder = WebApplication.CreateBuilder(args);


// CORS
var allowOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new[] { "*" };
builder.Services.AddCors(options =>
{
    options.AddPolicy("default", policy =>
    {
        policy.WithOrigins(allowOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Add services to the container.

// HttpClient and GoogleMapsService
builder.Services.AddHttpClient();
builder.Services.AddScoped<MapManagement_API.Services.GoogleMapsService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
app.UseCors("default");
app.UseAuthorization();

app.MapControllers();

app.Run();
