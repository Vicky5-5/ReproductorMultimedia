using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Logica.Models
{
    public class Canciones
    {
        [Key]
        public int idCancion { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public string Artista { get; set; }
        [Required]
        public string Album { get; set; }

        [Range(typeof(TimeSpan), "00:00:00", "01:00:00", ErrorMessage = "La duración tiene que estar entre 0 y 30 minutos")]
        public TimeSpan Duracion { get; set; }
        public int NumeroReproducciones { get; set; }=0;
        public int NumeroLikes { get; set; } = 0;
        [NotMapped]
        public IFormFile ArchivoCancion { get; set; }
        public string RutaArchivo { get; set; } // Ruta del archivo
        public virtual ICollection<ListaReproduccion> ListaReproduccion { get; set; }

    }
}
