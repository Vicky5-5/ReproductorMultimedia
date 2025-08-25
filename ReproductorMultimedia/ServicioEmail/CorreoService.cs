using Logica.Servicios;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net.Mail;
using System.Text;

public class CorreoService
{
    private readonly SmtpConfiguracion _smtpSettings;

    public CorreoService(IOptions<SmtpConfiguracion> smtpOptions) { 
        _smtpSettings = smtpOptions.Value; 
    }

    //Creamos el método para enviar el correo y que funciona tanto para en alta y baja. Así ahorramos código y es más fácil de mantener
    private void EnviarCorreo(string destinatario, string asunto, string cuerpo)
    {
        if (string.IsNullOrWhiteSpace(_smtpSettings.EmailFrom))
            throw new InvalidOperationException("El campo 'EmailFrom' no está configurado.");

        var remitente = new MailAddress(_smtpSettings.EmailFrom, "Reproductor Música"); // 👈 Aquí
        var mail = new MailMessage
        {
            From = remitente,
            Subject = asunto,
            Body = cuerpo,
            BodyEncoding = Encoding.UTF8,
            IsBodyHtml = false
        };
        mail.To.Add(destinatario);

        var client = new SmtpClient(_smtpSettings.Host)
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
            string mensajeError = $"[{DateTime.Now}] Error al enviar correo a {destinatario}: {ex.Message}";
            string rutaRelativa = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "AdminLog.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(rutaRelativa));
            File.AppendAllText(rutaRelativa, mensajeError + Environment.NewLine);
        }
    }


    public void EnviarCorreoAlta(string emailNuevoUsuario)
    {
        string asunto = "Bienvenido a Reproductor Multimedia";
        string cuerpo = "Te has dado de alta correctamente.";
        EnviarCorreo(emailNuevoUsuario, asunto, cuerpo);
    }

    public void EnviarCorreoBaja(string emailNuevoUsuario)
    {
        string asunto = "Hasta pronto";
        string cuerpo = "Te has dado de baja correctamente. Contacta con el administrador si deseas volver a darte de alta.";
        EnviarCorreo(emailNuevoUsuario, asunto, cuerpo);
    }

}
