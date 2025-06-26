using Logica.Managers;
using Logica.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
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
        //Se declara solo lectura y contiene la instancia del LoginManager
        private readonly LoginManager _loginManager;

        //Creamos un constructor de inyección de dependencia para inyectar automática una instancia del LoginManager
        //Esto va con patrón Singleton
        public LoginController(LoginManager loginManager)
        {
            _loginManager = loginManager;
        }
        //Revisar bien
        [HttpPost]
        public ActionResult Entrar(string email, string password)
        {
            var usuario = _loginManager.Login(email, password);
            if (usuario != null)
            {
                if (usuario.Administrador)
                {
                    Response.Cookies.Append("Nombre", usuario.Nombre);
                    return RedirectToAction("Administrador", "Usuarios");
                }

                TempData["Usuario"] = usuario.Nombre;
                return RedirectToAction("Index", "Canciones");
            }

            return RedirectToAction("Login");
        }

        [HttpGet, ActionName("VerUsuarios")]
        public ActionResult VerUsuarios()
        {

            return RedirectToAction("Administrador", "Usuarios");
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
                    var login = UsuarioViewModel.RegistroUsuarioNuevo(model.idUsuario, model.Nombre, model.Email, model.Password);
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
