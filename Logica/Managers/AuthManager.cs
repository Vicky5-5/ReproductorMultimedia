using Azure.Core;
using Logica.Contexto;
using Logica.Modelos_Auxiliares;
using Logica.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Managers
{
    public class AuthManager
    {
        private readonly Conexion _dbContext;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly CorreoService _emailService;
        public AuthManager(Conexion cn, IHttpContextAccessor contextAccessor, CorreoService correoServicio)
        {
            _dbContext = cn;
            _contextAccessor = contextAccessor;
            _emailService = correoServicio;
        }
        // Comprobamos que el dispositivo es nuevo
        public bool EsNuevoDispositivo(int userId)
        {
            var deviceId = _contextAccessor.HttpContext.Request.Cookies["device_id"];
            if (string.IsNullOrEmpty(deviceId)) return true;

            return !_dbContext.DispositivoUsuario.Any(d => d.idUsuario == userId && d.idDispositivo == deviceId);
        }
        // Generamos un código de verificación aleatorio de 5 dígitos
        private string GenerarCodigo()
        {
            Random random = new Random();
            return random.Next(10000, 99999).ToString();
        }
        // Para más seguridad guardamos un hash del código en la base de datos en lugar del código en texto plano
        private string GenerarHash(string codigo)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(codigo);
                byte[] hashBytes = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
        public void GenerarYEnviarCodigo(int userId, string email)
        {
            string codigo = GenerarCodigo();
            string hash = GenerarHash(codigo);

            _dbContext.LoginVerificacion.Add(new VerificacionLogin
            {
                idUsuario = userId,
                Hash = hash,
                Expiracion = DateTime.UtcNow.AddMinutes(10)
            });

            _dbContext.SaveChanges();

            _emailService.EnviarCodigoVerificacion(email, codigo);
        }
        public bool VerificarCodigo(int userId, string codigoIngresado)
        {
            var verification = _dbContext.LoginVerificacion
                .FirstOrDefault(v => v.idUsuario == userId && v.Expiracion > DateTime.UtcNow);

            if (verification == null) return false;

            if (GenerarHash(codigoIngresado) == verification.Hash)
            {
                RegistrarDispositivo(userId);
                _dbContext.LoginVerificacion.Remove(verification); // eliminar el código usado
                _dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        private void RegistrarDispositivo(int userId)
        {
            string deviceId = Guid.NewGuid().ToString();
            _contextAccessor.HttpContext.Response.Cookies.Append("device_id", deviceId);

            _dbContext.DispositivoUsuario.Add(new DispositivoUsuario
            {
                idUsuario = userId,
                idDispositivo = deviceId
            });

            _dbContext.SaveChanges();
        }
    }
}
