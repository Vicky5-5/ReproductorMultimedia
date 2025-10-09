using Logica.Contexto;
using Logica.Managers;
using Logica.Modelos_Auxiliares;
using Logica.Models;
using Logica.Spotify;
using Logica.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Net.Http.Headers;

namespace ReproductorMultimedia.Controllers
{
    public class VistaUsuarioController : Controller
    {
        private readonly LoginManager _loginManager;
        private readonly IConfiguration _configuration;
        public VistaUsuarioController(LoginManager loginManager, IConfiguration configuration)
        {
            _loginManager = loginManager;
            _configuration = configuration;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearListaNueva(string nombreLista, List<int> idsCanciones)
        {
            int? idUsuario = _loginManager.GetCurrentUserId();
            if (!idUsuario.HasValue)
                return RedirectToAction("Login", "Login");

            if (string.IsNullOrEmpty(nombreLista) || idsCanciones == null || !idsCanciones.Any())
            {
                ViewBag.Error = "Nombre de lista y canciones son obligatorios.";
                return View("Home");
            }

            try
            {
                var listaVM = ListaReproduccionViewModel.CrearLista(idUsuario.Value, nombreLista, idsCanciones);
                ViewBag.Mensaje = "Lista creada con éxito.";
                return View("VerTodasLasListas", listaVM);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al crear lista: {ex.Message}";
                return View("CrearLista");
            }
        }
        public IActionResult VerListaReproduccion()
        {
            return View();
        }

        [HttpGet]
        public IActionResult VerTodasLasListas()
        {
            int? idUsuario = _loginManager.GetCurrentUserId();
            if (!idUsuario.HasValue)
                return RedirectToAction("Login", "Login");

            var listas = ListaReproduccionViewModel.ObtenerListasPorUsuario(idUsuario.Value);
            return View("VerTodasLasListas", listas);
        }


        [HttpPost]
        // Se usa [FromBody] para que ASP.NET Core deserialice automáticamente los datos JSON enviados desde el cliente.
        // Usamos un objeto auxiliar para agrupar varios datos en una sola estructura, evitando múltiples parámetros y una URL sobrecargada.
        public IActionResult CrearLista([FromBody] ListaReproduccionAuxiliar auxiliar)
        {
            int? idUsuario = _loginManager.GetCurrentUserId();
            //Comprobamos que el usuario esté logueado
            if (!idUsuario.HasValue)
                return Json(new { success = false, mensaje = "Usuario no autorizado." });
            //Comprobamos que los datos estén completos
            if (string.IsNullOrWhiteSpace(auxiliar.nombreLista) || auxiliar.idsCanciones == null || !auxiliar.idsCanciones.Any())
                return Json(new { success = false, mensaje = "Datos incompletos." });

            try
            {
                //Creamos la lista
                var listaVM = ListaReproduccionViewModel.CrearLista(idUsuario.Value, auxiliar.nombreLista, auxiliar.idsCanciones);
                return View("VerTodasLasListas");
            }
            catch (Exception ex)
            {
                return View("Home");
            }
        }
        [HttpPost]
        public IActionResult BorrarLista(Guid idLista)
        {
            var usuario = UsuarioViewModel.GetUsuario(_loginManager.GetCurrentUserId() ?? 0);
            ListaReproduccionViewModel.BorrarLista(usuario.idUsuario, idLista);
            return View("Home");
        }

        [HttpGet]
        public IActionResult VerContenidoListaReproduccion(Guid idLista)
        {
            int? idUsuario = _loginManager.GetCurrentUserId();
            //Validamos antes idUsuario para que no sea null
            if (!idUsuario.HasValue)
                return RedirectToAction("Login", "Login");

            var listado = ListaReproduccionViewModel.ListarCancionesDeUnaLista(idLista, idUsuario.Value);

            string nombreLista = ListaReproduccionViewModel.ObtenerNombreLista(idLista, idUsuario.Value);

            ViewBag.NombreLista = nombreLista;

            return View(listado);
        }
        public ActionResult ModoOnline()
        {
            return View();
        }
        public IActionResult PagoPayPal()
        {
            var paypalClientId = _configuration["PayPal:ClientId"];
            ViewBag.PayPalClientId = paypalClientId;
            return View();
        }

        // Inicia el proceso de autenticación con Spotify. El usuario autorizaes redireccionado a Spotify y luego de autorizar es redirigido a la URL de callback.
        public IActionResult LoginSpotify()
        {
            // Leer configuración desde appsettings.json

            var clientId = _configuration["Spotify:ClienteID"]; // Identificador del cliente
            var redirectUri = _configuration["Spotify:RedirectUri"]; // URL de redirección registrada en la aplicación de Spotify
            var scopes = "user-read-private user-read-email user-read-playback-state user-modify-playback-state"; // Permisos solicitados

            var url = $"https://accounts.spotify.com/authorize?client_id={clientId}" +
                      $"&response_type=code" +
                      $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                      $"&scope={Uri.EscapeDataString(scopes)}";

            return Redirect(url);
        }
        // Reicbe el código de autorización de Spotify y obtiene el token de acceso.
        public ActionResult Callback(string code)
        {
            // Obtener configuración desde appsettings.json

            var clientId = _configuration["Spotify:ClienteID"];
            var clientSecret = _configuration["Spotify:ClienteSecret"];
            var redirectUri = _configuration["Spotify:RedirectUri"];

            // Creamos la solicitud HTTP para obtener el token

            using var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

            // Configuramos los datos del formulario POST
            var postData = new List<KeyValuePair<string, string>>
    {
        new("grant_type", "authorization_code"), // Tipo de autorización
        new("code", code), // Código recibido de Spotify
        new("redirect_uri", redirectUri), // Debe coincidir con el registrado
        new("client_id", clientId), // ID del cliente
        new("client_secret", clientSecret) // Código secreto del cliente
    };
            // Convertimos los datos a formato x-www-form-urlencoded

            request.Content = new FormUrlEncodedContent(postData);

            // Enviamos la solicitud y obtenemos la respuesta

            var response = client.Send(request);
            var content = response.Content.ReadAsStringAsync().Result;

            // Deserializamos la respuesta JSON para obtener el token

            var tokenResponse = JsonConvert.DeserializeObject<SpotifyTokenResponse>(content);

            // Guardamos el token en la sesión para usarlo en futuras solicitudes a la API de Spotify
            HttpContext.Session.SetString("SpotifyAccessToken", tokenResponse.access_token);

            return RedirectToAction("ModoOnline");
        }

        public ActionResult PerfilSpotify()
        {
            // Usamos el token de acceso guardado en la sesión para hacer una solicitud a la API de Spotify
            var token = HttpContext.Session.GetString("SpotifyAccessToken");
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = client.GetAsync("https://api.spotify.com/v1/me").Result;
            var content = response.Content.ReadAsStringAsync().Result;

            // Deserializamos la respuesta JSON para obtener los datos del perfil

            var perfil = JsonConvert.DeserializeObject<SpotifyPerfilResponse>(content);

            // Pasamos los datos del perfil a la vista usando ViewBag
            ViewBag.Nombre = perfil.NombreVisible;

            return View();
        }

        public ActionResult RenovarTokenSpotify()
        {
            // Usamos el refresh token guardado en la sesión para obtener un nuevo token de acceso
            var refreshToken = HttpContext.Session.GetString("SpotifyRefreshToken");
            var clientId = _configuration["Spotify:ClienteID"];
            var clientSecret = _configuration["Spotify:ClienteSecret"];

            using var client = new HttpClient();
            var postData = new List<KeyValuePair<string, string>>
    {
        new("grant_type", "refresh_token"),
        new("refresh_token", refreshToken),
        new("client_id", clientId),
        new("client_secret", clientSecret)
    };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token")
            {
                Content = new FormUrlEncodedContent(postData)
            };

            var response = client.Send(request);
            var content = response.Content.ReadAsStringAsync().Result;

            var tokenResponse = JsonConvert.DeserializeObject<SpotifyTokenResponse>(content);
            HttpContext.Session.SetString("SpotifyAccessToken", tokenResponse.access_token);

            ViewBag.TokenRenovado = tokenResponse.access_token;
            return View("PerfilSpotify");
        }
    }
}
