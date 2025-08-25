using Logica.Servicios;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net.Mail;
using System.Text;

public class CorreoService
{
    private readonly SmtpConfiguracion _smtpSettings;

    public CorreoService(IOptions<SmtpConfiguracion> smtpOptions)
    {
        _smtpSettings = smtpOptions.Value;
    }

    public void EnviarCorreoAlta(string emailNuevoUsuario)
    {
        string asunto = "Bienvenido a Reproductor Multimedia";
        string cuerpo = "Te has dado de alta correctamente";

        MailMessage mail = new MailMessage(_smtpSettings.EmailFrom, emailNuevoUsuario, asunto, cuerpo)
        {
            BodyEncoding = Encoding.UTF8,
            IsBodyHtml = false
        };

        SmtpClient client = new SmtpClient(_smtpSettings.Host)
        {
            EnableSsl = _smtpSettings.EnableSsl,
            UseDefaultCredentials = false,
            Port = _smtpSettings.Port,
            Credentials = new System.Net.NetworkCredential(_smtpSettings.EmailFrom, _smtpSettings.AppPassword)
        };

        try
        {
            client.Send(mail);
        }
        //Para regitrar el error en un fichero de texto
        catch (Exception ex)
        {
            string mensajeError = $"[{DateTime.Now}] Error al enviar correo a {emailNuevoUsuario}: {ex.Message}";

            string rutaRelativa = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "AdminLog.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(rutaRelativa));

            System.IO.File.AppendAllText(rutaRelativa, mensajeError + Environment.NewLine);
        }
    }
    public void EnviarCorreoBaja(string emailNuevoUsuario)
    {
        string asunto = "Hasta pronto";
        string cuerpo = "Te has dado de baja correctamente. Contacte con el administrador para darte de alta de nuevo";

        MailMessage mail = new MailMessage(_smtpSettings.EmailFrom, emailNuevoUsuario, asunto, cuerpo)
        {
            BodyEncoding = Encoding.UTF8,
            IsBodyHtml = false
        };

        SmtpClient client = new SmtpClient(_smtpSettings.Host)
        {
            EnableSsl = _smtpSettings.EnableSsl,
            UseDefaultCredentials = false,
            Port = _smtpSettings.Port,
            Credentials = new System.Net.NetworkCredential(_smtpSettings.EmailFrom, _smtpSettings.AppPassword)
        };

        try
        {
            client.Send(mail);
        }
        catch (Exception ex)
        {
            string mensajeError = $"[{DateTime.Now}] Error al enviar correo a {emailNuevoUsuario}: {ex.Message}";

            string rutaRelativa = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "AdminLog.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(rutaRelativa));

            System.IO.File.AppendAllText(rutaRelativa, mensajeError + Environment.NewLine);
        }
    }
}
