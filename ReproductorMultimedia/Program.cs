using Logica.Contexto;
using Logica.Managers;
using Logica.Servicios;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

//Para la conexión a la base de datos. Para que se ejecute al iniciiar el programa
//Se configura aquii antes construir la app
builder.Services.AddDbContext<Conexion>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Reproductor")));

// Add services to the container.
builder.Services.AddControllersWithViews();

//Para controlar el el http current
builder.Services.AddHttpContextAccessor(); // Permite el acceso a HttpContext

//Se registra con un servicio inyectable en el contenedor de dependenica
//Para que pueda ser inyectada en culaquier controlado o servicio. El patrón Sinleton es una instancia GLOBAL
builder.Services.AddScoped<LoginManager>();
builder.Services.AddScoped<CorreoService>(); //Para que el servicio de correo pueda ser inyectado

//Configuración de la sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); //Tiempo de inactividad antes de expirar
    options.Cookie.HttpOnly = true; //Solo accesible por el servidor
    options.Cookie.IsEssential = true; //Esencial para la funcionalidad
});

//Política de cookies segura
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.Secure = CookieSecurePolicy.Always; //Solo sobre HTTPS
});

//Configuración del servicio SMTP para envío de correos
builder.Services.Configure<SmtpConfiguracion>(
    builder.Configuration.GetSection("SmtpConfiguracion"));

//Configuración de localización para que la cultura por defecto sea español (España)
//Esto afecta a fechas, números, validaciones y codificación de texto
var supportedCultures = new[] { new CultureInfo("es-ES") };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("es-ES");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); //Manejo de errores en producción
    app.UseHsts(); //Seguridad HTTP Strict Transport
}

app.UseHttpsRedirection(); //Redirección a HTTPS
app.UseStaticFiles(); //Archivos estáticos como CSS, JS, imágenes

app.UseSession(); //Activación de sesiones

app.UseRouting(); //Enrutamiento de solicitudes

app.UseAuthorization(); //Autorización de usuarios

//Activación de la localización configurada anteriormente
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

//Definición de rutas por defecto
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); //Inicio de la aplicación
