using Logica.Managers;
using Logica.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReproductorMultimedia.Controllers
{
    public class VistaUsuarioController : Controller
    {
        // GET: VistaUsuarioController
        public IActionResult Home()
        {
            var lista = CancionesViewModel.ListSongs();
            return View(lista);
        }

        // GET: VistaUsuarioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public IActionResult Likes(int id)
        {
            CancionesManager.ActualizarLikes(id);

            var canciones = CancionesViewModel.ListSongs();
            return View("Home", canciones);
        }

    }
}
