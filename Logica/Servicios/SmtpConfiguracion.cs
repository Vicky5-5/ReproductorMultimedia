using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Servicios
{
    public class SmtpConfiguracion
    {
        public string EmailFrom { get; set; }
        public string AppPassword { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
    }
}
