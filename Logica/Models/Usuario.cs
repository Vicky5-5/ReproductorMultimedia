using System;
using System.Collections.Generic;
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

        public string Email { get; set; }
        public string Password { get; set; }
        public bool Estado { get; set; }
        public DateTime fechaRegistro { get; set; }
        public DateTime? fechaBaja { get; set; }
        public bool Administrador { get; set; }
        public virtual ICollection<ListaReproduccion> ListasReproduccion { get; set; }

        public Usuario()
        {
            fechaRegistro = DateTime.Now;
        }
    }

}
