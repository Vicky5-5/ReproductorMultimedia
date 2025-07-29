using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Models
{
    public class Usuario
    {
        [Key]
        public int idUsuario { get; set; }

        [Required]
        public string Nombre { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]

        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 4000, MinimumLength = 10)]

        public string Password { get; set; }
        public bool Estado { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime fechaRegistro { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]

        public DateTime? fechaBaja { get; set; }
        public bool Administrador { get; set; }
        public virtual ICollection<ListaReproduccion> ListasReproduccion { get; set; }
               
    }

}
