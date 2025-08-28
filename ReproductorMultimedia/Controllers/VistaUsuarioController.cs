using Logica.Managers;
using Logica.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace ReproductorMultimedia.Controllers
{
    public class VistaUsuarioController : Controller
    {
        private readonly LoginManager _loginManager;

        public VistaUsuarioController(LoginManager loginManager)
        {
            _loginManager = loginManager;
        }

        // GET: VistaUsuarioController
        public IActionResult Home()
        {
            string nombreUsuario = _loginManager.GetCurrentUser();
            int? idUsuario = _loginManager.GetCurrentUserId();

            // Llama a la versión correcta que incluye los corazones rojos
            var lista = CancionesViewModel.ListarFavoritasComoCanciones(idUsuario);

            ViewBag.NombreUsuario = nombreUsuario;
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
        [HttpPost]
        public IActionResult LikeAlternar([FromBody] int idCancion)
        {
            int? idUsuario = _loginManager.GetCurrentUserId();

            if (idCancion <= 0)
            {
                return Json(new
                {
                    success = false,
                    mensaje = "ID de canción inválido."
                });
            }

            if (idUsuario == null)
            {
                return Json(new
                {
                    success = false,
                    mensaje = "Debes iniciar sesión para dar like."
                });
            }

            bool dioLike = CancionesViewModel.LikeDislike(idUsuario.Value, idCancion);
            int likesTotales = CancionesViewModel.UpdateLikes(idCancion);

            return Json(new
            {
                success = true,
                dioLike = dioLike,
                mensaje = dioLike ? "Like agregado" : "Like quitado",
                likesTotales = likesTotales
            });
        }

        public IActionResult FavoritasUsuario()
        {
            int? idUsuario = _loginManager.GetCurrentUserId();

            if (!idUsuario.HasValue)
                return RedirectToAction("Login", "Login");

            var favoritas = CancionesFavoritasViewModel.ListarFavoritasPorUsuario(idUsuario.Value);

            ViewBag.NombreUsuario = _loginManager.GetCurrentUser();
            return View(favoritas);
        }
        [HttpGet]
        public IActionResult DarseBaja()
        {
            int? idUsuario = _loginManager.GetCurrentUserId();

            if (!idUsuario.HasValue)
                return RedirectToAction("Login", "Login");

            var usuario = UsuarioViewModel.DatosUnUsuario(idUsuario.Value);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DarseBajaAccion()
        {
            int? idUsuario = _loginManager.GetCurrentUserId();

            if (!idUsuario.HasValue)
                return RedirectToAction("Login", "Login");

            UsuarioViewModel.BajaVoluntaria(idUsuario.Value);


            return RedirectToAction("Login", "Login");
        }



    }
}
