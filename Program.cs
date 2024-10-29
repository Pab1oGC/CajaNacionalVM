using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using CNSVM.Data;
using Supabase;
using CNSVM.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CnsvmDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CnsvmConnection")));

builder.Services.AddDbContext<SiaisDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SiaisConnection")));

builder.Services.AddDbContext<ErpcnsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ErpcnsConnection")));

builder.Services.AddHttpClient();

//Configuraci�n Supabase
var supabaseUrl = builder.Configuration["Supabase:Url"];
var supabaseKey = builder.Configuration["Supabase:Key"];
var client = new Client(supabaseUrl, supabaseKey);
client.InitializeAsync().Wait();
builder.Services.AddSingleton(client);
builder.Services.AddScoped<SupabaseService>();

// Configuraci�n de autenticaci�n con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Users/Login";  // Redirigir a la p�gina de inicio de sesi�n
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);  // Tiempo de expiraci�n de la cookie
        options.SlidingExpiration = true;  // Renueva el tiempo de expiraci�n con cada solicitud
    });

// Agregar servicios al contenedor
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  // Habilitar autenticaci�n
app.UseAuthorization();   // Habilitar autorizaci�n

app.MapRazorPages();
app.MapGet("/", context =>
{
    context.Response.Redirect("/Users/Login");
    return Task.CompletedTask;
});




app.Run();
