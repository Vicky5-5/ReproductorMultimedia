using System;
using Logica.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

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
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Session.SetString("Fallido", "Faltan datos por insertar");
                return null;
            }

            if (bloqueo && tiempoBloqueo.HasValue && DateTime.Now < tiempoBloqueo.Value)
            {
                Session.SetString("Mensaje", "Cuenta bloqueada. Inténtelo nuevamente después de 10 minutos.");
                return null;
            }

            var login = UsuariosManager.Login(email, password); // Validación real contra la base de datos
            var hash = UsuariosManager.HashPassword(password);

            int intentosFallidos = Session.GetInt32("IntentosFallidos") ?? 0;

            if (login == null || login.Password != hash || login.Email != email || !login.Estado)
            {
                intentosFallidos++;
                Session.SetInt32("IntentosFallidos", intentosFallidos);
                Session.SetString("Incorrecto", "La contraseña o el email están incorrectos. Inténtalo de nuevo.");

                if (intentosFallidos >= 3)
                {
                    bloqueo = true;
                    tiempoBloqueo = DateTime.Now.AddMinutes(10);
                    Session.SetString("Mensaje", "Se ha superado el número de intentos. Inténtelo de nuevo en 10 minutos.");
                }

                return null;
            }

            Session.SetString("Bienvenida", $"Bienvenido/a: {login.Nombre}");
            Session.SetString("UsuarioActual", login.Nombre);
            Session.SetInt32("UsuarioID", login.idUsuario);
            Session.SetInt32("IntentosFallidos", 0);
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
