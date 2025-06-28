using Logica.Managers;
using Logica.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ReproductorMultimedia.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginManager _loginManager;

        public LoginController(LoginManager loginManager)
        {
            _loginManager = loginManager;
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [ActionName("LogOut")]
        public ActionResult LogOut()
        {
            HttpContext.Session.Clear(); // Limpia todos los datos de la sesión
            Response.Cookies.Delete("Nombre"); // Elimina la cookie si la estás usando
            return RedirectToAction("Login", "Login");
        }


        [HttpPost]
        public ActionResult Entrar(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                TempData["Error"] = "Por favor, ingrese su correo electrónico y contraseña.";
                return RedirectToAction("Login");
            }

            try
            {
                var usuario = _loginManager.Login(email, password);
                if (usuario != null)
                {
                    // Almacenar el nombre del usuario en la sesión
                    HttpContext.Session.SetString("Nombre", usuario.Nombre);

                    if (usuario.Administrador)
                    {
                        return RedirectToAction("Administrador", "Usuario");
                    }

                    TempData["Usuario"] = usuario.Nombre;
                    return RedirectToAction("Index", "Canciones");
                }

                TempData["Error"] = "Correo electrónico o contraseña incorrectos.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al iniciar sesión: {ex.Message}";
            }

            return RedirectToAction("Login");
        }


        [HttpGet, ActionName("VerUsuarios")]
        public ActionResult VerUsuarios()
        {
            return RedirectToAction("Administrador", "Usuario");
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
                    // Redirigir a Login después de un registro exitoso
                    return RedirectToAction("Login", "Login");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"Error al guardar: {ex.Message}";
                }
            }

            // Si el modelo no es válido o hay un error, volver a mostrar el formulario
            return View(model);
        }

    }
}
