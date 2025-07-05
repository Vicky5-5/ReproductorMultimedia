using System;
using Logica.Contexto;
using Logica.Managers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Para la conexión a la base de datos. Para que se ejecute al iniciiar el programa
//Se configura aquii antes construir la app

builder.Services.AddDbContext<Conexion>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Reproductor")));

// Add services to the container.
builder.Services.AddControllersWithViews();
//Para controlar el el http current
builder.Services.AddHttpContextAccessor();// Permite el acceso a HttpContext

//Se registra con un servicio inyectable en el contenedor de dependenica
//Para que pueda ser inyectada en culaquier controlado o servicio. El patrón Sinleton es una instancia GLOBAL
builder.Services.AddScoped<LoginManager>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.Secure = CookieSecurePolicy.Always;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
//para la sesiones (Session)
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

