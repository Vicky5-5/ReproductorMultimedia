using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Models
{
    public class Canciones
    {
        [Key]
        public int idCancion { get; set; }
        public string Titulo { get; set; }
        public string Artista { get; set; }
        public string Album { get; set; }

        [Range(typeof(TimeSpan), "00:00:00", "01:00:00", ErrorMessage = "La duración tiene que estar entre 0 y 30 minutos")]
        public TimeSpan Duracion { get; set; }
        public Boolean Favorito { get; set; }
        public virtual ICollection<ListaReproduccion> ListaReproduccion { get; set; }

    }
}
