using System;
using Logica.Models;
using Microsoft.AspNetCore.Http;

namespace Logica.Managers
{
    public sealed class LoginManager
    {
        private static LoginManager instance = null;
        private readonly IHttpContextAccessor _contextAccessor;
        private string currentUser;
        private bool bloqueo = false;
        private DateTime? tiempoBloqueo = null;

        // Constructor privado para evitar instanciación externa. Este es el patrón Singleton
        public LoginManager(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public static LoginManager Instance(IHttpContextAccessor contextAccessor)
        {
            if (instance == null)
            {
                instance = new LoginManager(contextAccessor);
            }
            return instance;
        }

        public Usuario Login(string email, string password)
        {
            var session = _contextAccessor.HttpContext.Session; //Usamos la sesión actual para guardar los datos del usuario

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                session.SetString("Fallido", "Faltan datos por insertar");
                return null;
            }

            // Verificar si el usuario está bloqueado por tiempo (por email)
            var tiempoBloqueoString = session.GetString($"TiempoBloqueo_{email}");
            if (!string.IsNullOrEmpty(tiempoBloqueoString) &&
                DateTime.TryParse(tiempoBloqueoString, out DateTime tiempoBloqueoStored))
            {
                if (DateTime.Now < tiempoBloqueoStored)
                {
                    session.SetString("Mensaje", "Cuenta bloqueada. Inténtelo nuevamente después de 10 minutos.");
                    return null;
                }
                else
                {
                    // El tiempo de bloqueo ha expirado, limpiamos
                    session.Remove($"TiempoBloqueo_{email}");
                    session.SetInt32($"IntentosFallidos_{email}", 0);
                }
            }

            var login = UsuariosManager.Login(email, password);
            var hash = UsuariosManager.HashPassword(password);

            int intentosFallidos = session.GetInt32($"IntentosFallidos_{email}") ?? 0;

            if (login == null || hash != login.Password || email != login.Email || !login.Estado)
            {
                intentosFallidos++;
                session.SetInt32($"IntentosFallidos_{email}", intentosFallidos);

                if (intentosFallidos >= 3)
                {
                    var bloqueoHasta = DateTime.Now.AddMinutes(10);
                    session.SetString($"TiempoBloqueo_{email}", bloqueoHasta.ToString("o")); // formato ISO 8601
                    session.SetString("Mensaje", "Se ha superado el número de intentos. Inténtelo de nuevo en 10 minutos.");
                }
                else
                {
                    session.SetString("MensajeError", "Usuario o contraseña incorrecto.");
                }

                return null;
            }

            // Si llega aquí, login correcto
            session.SetString("Bienvenida", $"Bienvenido/a: {login.Nombre}");
            session.SetInt32("idUsuario", login.idUsuario);
            session.SetString("Nombre", login.Nombre); // Guardar nombre en sesión
            session.SetInt32($"IntentosFallidos_{email}", 0); // Reinicia intentos
            session.Remove($"TiempoBloqueo_{email}"); // Quita el bloqueo si lo había

            currentUser = login.Nombre;
            return login;
        }

        //Obtenemos el nombre del usuario actual
        public string GetCurrentUser()
        {
            return _contextAccessor.HttpContext.Session.GetString("Nombre");
        }
        // Para saber si el usuario está logueado
        public int? GetCurrentUserId()
        {
            return _contextAccessor.HttpContext.Session.GetInt32("idUsuario");
        }

        public void Logout()
        {
            var session = _contextAccessor.HttpContext.Session;
            session.Clear();
            currentUser = null;
        }
    }
}
