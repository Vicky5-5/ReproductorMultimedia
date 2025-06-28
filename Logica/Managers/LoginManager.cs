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

        // Constructor privado para evitar instanciación externa
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
            var session = _contextAccessor.HttpContext.Session;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                session.SetString("Fallido", "Faltan datos por insertar");
                return null;
            }

            if (bloqueo && tiempoBloqueo.HasValue && DateTime.Now < tiempoBloqueo.Value)
            {
                session.SetString("Mensaje", "Cuenta bloqueada. Inténtelo nuevamente después de 10 minutos.");
                return null;
            }

            var login = UsuariosManager.Login(email, password);
            var hash = UsuariosManager.HashPassword(password);

            // Almacenamos los intentos de inicio de sesión
            int intentosFallidos = session.GetInt32("IntentosFallidos") ?? 0;

            if (login == null)
            {
                session.SetString("MensajeError", "Error. Usuario o contraseña incorrecto.");
                return null;
            }

            if (hash == login.Password && email == login.Email && login.Estado)
            {
                currentUser = login.Nombre;
                session.SetString("Bienvenida", $"Bienvenido/a: {login.Nombre}");
                session.SetInt32("IntentosFallidos", 0); // Reinicia intentos
                return login; // Login exitoso
            }
            else
            {
                intentosFallidos++;
                session.SetString("Incorrecto", "La contraseña o el email están incorrectos. Inténtalo de nuevo.");
                session.SetInt32("IntentosFallidos", intentosFallidos);
            }

            if (intentosFallidos >= 3)
            {
                bloqueo = true;
                tiempoBloqueo = DateTime.Now.AddMinutes(10);
                session.SetString("Mensaje", "Se ha superado el número de intentos. Inténtelo de nuevo en 10 minutos.");
            }

            return null; // Login fallido
        }

        public string GetCurrentUser()
        {
            return currentUser;
        }

        public void Logout()
        {
            var session = _contextAccessor.HttpContext.Session;
            session.Clear();
            currentUser = null; // Reinicia el usuario actual
        }
    }
}
