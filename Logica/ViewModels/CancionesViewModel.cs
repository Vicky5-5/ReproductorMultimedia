using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logica.Models;

namespace Logica.ViewModels
{
    public class CancionesViewModel
    {
        public int idCancion { get; set; }
        public string Titulo { get; set; }
        public string Artista { get; set; }
        public string Album { get; set; }

        [Range(typeof(TimeSpan), "00:00:00", "01:00:00", ErrorMessage = "La duración tiene que estar entre 0 y 30 minutos")]
        public TimeSpan Duracion { get; set; }
        public int NumeroReproducciones { get; set; } = 0;
        public int NumeroLikes { get; set; } = 0;
        public virtual ICollection<ListaReproduccion> ListaReproduccion { get; set; }
    }
}
