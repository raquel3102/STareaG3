using STarea.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

//Registra ApiService con HttpClient y URL base de tu API
builder.Services.AddHttpClient<ApiService>(client =>
{
    var apiUrl = builder.Configuration["Start:ApiUrl"];
    if (string.IsNullOrEmpty(apiUrl))
    {
        throw new ArgumentNullException(nameof(apiUrl), "La URL del API no puede ser nula.");
    }

    client.BaseAddress = new Uri(apiUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Principal}/{action=Index}/{id?}");

app.Run();