using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReproductorMultimedia.Models;

namespace ReproductorMultimedia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CorreoService _correoService;

        public HomeController(ILogger<HomeController> logger, CorreoService correoService)
        {
            _logger = logger;
            _correoService = correoService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Contacto()
        {
            return View();
        }
        [HttpPost]
        public IActionResult FormularioContacto(string nombre, string email, string asunto, string mensaje)
        {

            _correoService.EnviarCorreoContacto(nombre, email, asunto, mensaje);

            return View();
        }
    }
}
