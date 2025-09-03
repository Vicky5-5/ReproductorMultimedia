using Logica.Managers;
using Logica.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ReproductorMultimedia.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginManager _loginManager;
        //Para enviar el correo
        private readonly CorreoService _correoService;

        public LoginController(LoginManager loginManager, CorreoService correoService)
        {
            _loginManager = loginManager;
            _correoService = correoService;
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
        [ValidateAntiForgeryToken]
        public ActionResult Entrar(string email, string password)
        {
          
            var viewModel = new UsuarioViewModel
            {
                Email = email,
                Password = password
            };

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Por favor, ingrese su correo electrónico y contraseña.");
                return View("Login", viewModel);
            }

            try
            {
                var usuario = _loginManager.Login(email, password);
                if (usuario != null)
                {
                    if (!usuario.Estado)
                    {
                        ModelState.AddModelError("", "Tu cuenta está inactiva. Por favor, contacta al administrador.");
                        return View("Login", viewModel);
                    }

                    HttpContext.Session.SetString("Nombre", usuario.Nombre);

                    if (usuario.Administrador)
                        return RedirectToAction("Administrador", "Usuario");

                    TempData["Usuario"] = usuario.Nombre;
                    return RedirectToAction("Home", "VistaUsuario");
                }

                ModelState.AddModelError("", "Correo electrónico o contraseña incorrectos.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al iniciar sesión: {ex.Message}");
            }

            return View("Login", viewModel);
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

        [HttpPost, ActionName("Registro")]
        public ActionResult Registro(UsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var login = UsuarioViewModel.RegistroUsuarioNuevo(model.idUsuario, model.Nombre, model.Email, model.Password);
                    _correoService.EnviarCorreoAlta(model.Email);

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
