using Logica.Managers;
using Logica.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace ReproductorMultimedia.Controllers
{

    // GET: LoginController1
    public class LoginController : Controller
    {

        // GET: Login
        public ActionResult Login()
        {

            return View();
        }
        [ActionName("LogOut")]
        public ActionResult LogOut()
        {
            return RedirectToAction("Login", "Login");

        }
        //Revisar bien
        [HttpPost]
        public ActionResult Entrar(string email, string password, UsuarioViewModel entrar)
        {
            LoginManager loginManager = LoginManager.Instance;
            var usuario = loginManager.Login(email, password);
            if (usuario != null)
            {
                if (usuario.Administrador == true)
                {

                    HttpCookie cookie = new HttpCookie("Nombre", entrar.Nombre); // Para sustituir el Temp Data
                    var miCookie = ControllerContext.HttpContext.Request.Cookies["Nombre"];
                    if (miCookie != null)
                    {

                        var nombre = loginManager.GetCurrentUser();
                    }

                    return RedirectToAction("Index", "Usuarios");
                }
                else
                {
                    TempData["Usuario"] = entrar.Nombre; // Creamos un TempData para que cuando el usuario inicie sesión se vea su nombre
                    return RedirectToAction("Index", "Canciones");
                }

            }

            return RedirectToAction("Login", "Login");
        }

        [HttpGet, ActionName("VerUsuarios")]
        public ActionResult VerUsuarios()
        {

            return RedirectToAction("Index", "Usuarios");
        }
        public ActionResult Registro()
        {
            return View();
        }

        [HttpPost, ActionName("Registrar")]
        public ActionResult Registro(UsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var login = UsuarioViewModel.RegistroUsuarioNuevo(model.idUsuario, model.Nombre, model.Email, model.Password, model.Direccion);
                }
                catch (Exception ex)
                {

                    ViewBag.Error = $"Error al guardar: {ex.Message}";
                }
            }

            return RedirectToAction("Login", "Login");
        }


    }
}
