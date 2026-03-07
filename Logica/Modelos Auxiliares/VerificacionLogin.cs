using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Modelos_Auxiliares
{
    public class VerificacionLogin
    {
        public int Id { get; set; }
        public int idUsuario { get; set; }
        public string Hash { get; set; }
        public DateTime Expiracion { get; set; }
    }
}
