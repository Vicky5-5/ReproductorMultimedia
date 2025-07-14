using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Models
{
    public class CancionesFavoritas
    {
        [Key]
        public int idFavorita { get; set; } // Clave primaria para la tabla Favoritos
        public int idUsuario { get; set; }
        public DateTime fecharAnadidaFavorita { get; set; } = DateTime.Now; // Fecha en que se añadió la canción a favoritos
        public virtual Usuario Usuario { get; set; }
        public int idCancion { get; set; }
        public virtual Canciones Cancion { get; set; }
    }
}
