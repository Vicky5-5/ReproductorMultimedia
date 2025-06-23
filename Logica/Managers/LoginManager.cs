using System;
using Logica.Models;
using Microsoft.AspNetCore.Http;

namespace Logica.Managers
{
    public sealed class LoginManager
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private bool bloqueo = false;
        private DateTime? tiempoBloqueo = null;

        public LoginManager(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        private ISession Session => _contextAccessor.HttpContext.Session;

        public Usuario Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                Session.SetString("Fallido", "Faltan datos por insertar");
                return null;
            }

            var tiempoBloqueoStr = Session.GetString("TiempoBloqueo");
            if (!string.IsNullOrEmpty(tiempoBloqueoStr) &&
                DateTime.TryParse(tiempoBloqueoStr, out var tiempoBloqueo) &&
                DateTime.Now < tiempoBloqueo)
            {
                Session.SetString("Mensaje", "Cuenta bloqueada. Inténtelo nuevamente después de 10 minutos.");
                return null;
            }

            var login = UsuariosManager.Login(email, password); // Validación real contra DB
            var hash = UsuariosManager.HashPassword(password);

            int intentosFallidos = Session.GetInt32("IntentosFallidos") ?? 0;

            if (login == null || login.Password != hash || login.Email != email || !login.Estado)
            {
                intentosFallidos++;
                Session.SetInt32("IntentosFallidos", intentosFallidos);
                Session.SetString("Incorrecto", "La contraseña o el email están incorrectos. Inténtalo de nuevo.");

                if (intentosFallidos >= 3)
                {
                    Session.SetString("TiempoBloqueo", DateTime.Now.AddMinutes(10).ToString());
                    Session.SetString("Mensaje", "Se ha superado el número de intentos. Inténtelo de nuevo en 10 minutos.");
                }

                return null;
            }

            // Login exitoso
            Session.SetString("Bienvenida", $"Bienvenido/a: {login.Nombre}");
            Session.SetString("UsuarioActual", login.Nombre);
            Session.SetInt32("UsuarioID", login.idUsuario);
            Session.SetInt32("IntentosFallidos", 0);
            Session.Remove("TiempoBloqueo"); // Reinicia bloqueo si existía
            return login;
        }


        public string GetCurrentUser()
        {
            return Session.GetString("UsuarioActual");
        }

        public void Logout()
        {
            Session.Clear();
        }
    }
}
