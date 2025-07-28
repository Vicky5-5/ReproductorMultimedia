using Logica.Managers;
using Logica.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReproductorMultimedia.Controllers
{
    public class VistaUsuarioController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor; //Se almacenará el contexto HTTP para acceder a la sesión y otros datos del usuario.
        //Constructor que recibe IHttpContextAccessor para acceder al contexto HTTP
        public VistaUsuarioController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        // GET: VistaUsuarioController
        public IActionResult Home()
        {
          
            var loginManager = LoginManager.Instance(_contextAccessor);
            string nombreUsuario = loginManager.GetCurrentUser();

            ViewBag.NombreUsuario = nombreUsuario;

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
        [HttpPost]
        public IActionResult LikeAlternar(int idCancion)
        {
            // Verificar si el usuario ha iniciado sesión
            var loginManager = LoginManager.Instance(_contextAccessor);
            int? idUsuario = loginManager.GetCurrentUserId();

            if (idUsuario == null)
            {
                return Json(new
                {
                    success = false,
                    mensaje = "Debes iniciar sesión para dar like."
                });
            }
            //Llamamos al metodo para alternar el like
            bool dioLike = CancionesViewModel.LikeDislike(idUsuario.Value, idCancion);
            // Actualizamos el total de likes de la canción
            var likesTotales = CancionesViewModel.UpdateLikes(idCancion);

            //Delvolvermos un JSON con la respuesta al usuario

            return Json(new
            {
                success = true,
                dioLike = dioLike,
                mensaje = dioLike ? "Like agregado" : "Like quitado",
                likesTotales = likesTotales
            });
        }


    }
}
