using SApiTarea.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Cargar configuración adicional según el entorno
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

// 💾 Servicios
builder.Services.AddScoped<DapperService>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("DefaultConnection");
    return new DapperService(connectionString);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 🧭 Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SApiTarea", Version = "v1" });
});

// 🌐 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// 🚀 Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SApiTarea v1");
        c.RoutePrefix = string.Empty; // Swagger en la raíz
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();