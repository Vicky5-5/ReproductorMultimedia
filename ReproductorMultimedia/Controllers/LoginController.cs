using Logica.Managers;
using Logica.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReproductorMultimedia.wwwroot.Captcha;
using System.Net.Http;

namespace ReproductorMultimedia.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginManager _loginManager;
        //Para enviar el correo
        private readonly CorreoService _correoService;

        private readonly IConfiguration _configuration;
        private static readonly HttpClient _httpClient = new HttpClient();

        public LoginController(LoginManager loginManager, CorreoService correoService, IConfiguration configuration)
        {
            _loginManager = loginManager;
            _correoService = correoService;
            _configuration = configuration;

        }


        // GET: Login
        public ActionResult Login()
        {
            ViewBag.SiteKey = _configuration["GoogleReCaptcha:SiteKey"]; // Pasar la clave del sitio a la vista
            return View();
        }

        [ActionName("LogOut")]
        public ActionResult LogOut()
        {
            HttpContext.Session.Clear(); // Limpia todos los datos de la sesión
            Response.Cookies.Delete("Nombre"); // Elimina la cookie si la estás usando
            return RedirectToAction("Login", "Login");
        }
        // Método para validar el reCAPTCHA v3
        private bool ValidarReCaptcha(string token)
        {
            var secretKey = _configuration["GoogleReCaptcha:SecretKey"]; // Obtener la clave secreta desde la configuración
            try
            {
                // Realizar la solicitud a la API de verificación de reCAPTCHA
                var response = _httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={token}", null)
                                          .GetAwaiter().GetResult();

                // Verificar si la respuesta fue exitosa. Si no, consideramos el captcha inválido
                if (!response.IsSuccessStatusCode)
                    return false;

                // Leer y deserializar la respuesta JSON de Google
                var responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject<ReCaptchaResponse>(responseString);

                return result?.success ?? false;
            }
            catch
            {
                // En caso de error (conexión, etc.) consideramos captcha inválido
                return false;
            }
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

            // Obtener el token de reCAPTCHA enviado desde el formulario
            var captchaToken = Request.Form["g-recaptcha-response"];

            // Verificar el token de reCAPTCHA si está vacío o inválido
            if (string.IsNullOrWhiteSpace(captchaToken) || !ValidarReCaptcha(captchaToken))
            {
                ViewBag.ModalTitulo = "Captcha inválido";
                ViewBag.ModalMensaje = "Por favor, verifica que no eres un robot.";
                ViewBag.ModalBoton = "Cerrar";
                ViewBag.MostrarModal = true;
                return View("Login", viewModel);
            }

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.ModalTitulo = "Campos requeridos";
                ViewBag.ModalMensaje = "Por favor, ingrese su correo electrónico y contraseña.";
                ViewBag.ModalBoton = "Cerrar";
                ViewBag.MostrarModal = true;
                return View("Login", viewModel);
            }

            try
            {
                var usuario = _loginManager.Login(email, password);
                if (usuario != null)
                {
                    if (!usuario.Estado)
                    {
                        ViewBag.ModalTitulo = "Cuenta inactiva";
                        ViewBag.ModalMensaje = "Tu cuenta está inactiva. Por favor, contacta al administrador.";
                        ViewBag.ModalBoton = "Cerrar";
                        ViewBag.MostrarModal = true;
                        return View("Login", viewModel);
                    }

                    HttpContext.Session.SetString("Nombre", usuario.Nombre);

                    if (usuario.Administrador)
                        return RedirectToAction("Administrador", "Usuario");

                    TempData["Usuario"] = usuario.Nombre;
                    return RedirectToAction("Home", "VistaUsuario");
                }

                var mensajeError = HttpContext.Session.GetString("Mensaje") ?? HttpContext.Session.GetString("MensajeError");

                ViewBag.ModalTitulo = "Error de inicio de sesión";
                ViewBag.ModalMensaje = mensajeError ?? "Correo o contraseña incorrectos.";
                ViewBag.ModalBoton = "Cerrar";
                ViewBag.MostrarModal = true;
            }
            catch (Exception ex)
            {
                ViewBag.ModalTitulo = "Error inesperado";
                ViewBag.ModalMensaje = $"Error al iniciar sesión: {ex.Message}";
                ViewBag.ModalBoton = "Cerrar";
                ViewBag.MostrarModal = true;
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
                    //_correoService.EnviarCorreoAlta(model.Email);

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
