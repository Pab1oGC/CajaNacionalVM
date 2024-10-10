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
//Configuration Supabase
var supabaseUrl = builder.Configuration["Supabase:Url"];
var supabaseKey = builder.Configuration["Supabase:Key"];
var client = new Client(supabaseUrl, supabaseKey);
client.InitializeAsync().Wait();
builder.Services.AddSingleton(client);
builder.Services.AddScoped<SupabaseService>();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();
// Agregar los DbContext para las bases de datos cnsvm y siais


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapGet("/", context =>
{
	context.Response.Redirect("/Users/Login");
	return Task.CompletedTask;
});

app.Run();
