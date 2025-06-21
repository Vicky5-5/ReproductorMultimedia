using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Models
{
    public class ListaReproduccion
    {
        [Key]
        public int idLista { get; set; }
        [NotMapped]
        public string NombreLista { get; set; }

        public DateTime fechaCreacion { get; set; }
        //Clave foranea
        [NotMapped]

        public int idCancion { get; set; }
        [NotMapped]

        public int idUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Canciones Cancion { get; set; }

    }
}
