using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logica.Models
{
    public class ListaReproduccion
    {
        [Key]
        public Guid idLista { get; set; }

        [Required]
        public string NombreLista { get; set; }

        public DateTime fechaCreacion { get; set; } = DateTime.Now; // Fecha de creación de la lista
        public DateTime cancionAnadida { get; set; } = DateTime.Now; // Fecha en que se añadió la canción a la lista
        // Claves foráneas
        [ForeignKey("idCancion")]
        public int idCancion { get; set; }

        [ForeignKey("idUsuario")]
        public int idUsuario { get; set; }

        // Propiedades de navegación
        public virtual Usuario Usuario { get; set; }
        public virtual Canciones Cancion { get; set; }
    }
}
