using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Required]
        [RegularExpression(@"^([0-5]?\d):([0-5]\d)$", ErrorMessage = "Duración debe tener formato mm:ss")]

        public TimeSpan Duracion { get; set; }
        public Genero Genero { get; set; }

        public int NumeroReproducciones { get; set; }=0;
        public int NumeroLikes { get; set; } = 0;
        [NotMapped]
        public IFormFile ArchivoCancion { get; set; }
        public string RutaArchivo { get; set; } // Ruta del archivo
        public virtual ICollection<ListaReproduccion> ListaReproduccion { get; set; }

    }
    public enum Genero
    {
        [DescriptionAttribute("Pop")]
        Pop, //0

        [DescriptionAttribute("Rock")]
        Rock, //1

        [DescriptionAttribute("Electrónica")]
        Electronica //2
    }
}
