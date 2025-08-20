using Logica.Managers;
using Logica.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logica.ViewModels
{
    public class CancionesFavoritasViewModel
    {
        [Key]
        public int idFavorita { get; set; }

        [ForeignKey("Usuario")]
        public int idUsuario { get; set; }

        public DateTime fecharAnadidaFavorita { get; set; } = DateTime.Now;

        public virtual Usuario Usuario { get; set; }

        [ForeignKey("Cancion")]
        public int idCancion { get; set; }

        public virtual Canciones Cancion { get; set; }

        
        public CancionesFavoritasViewModel(CancionesFavoritas favoritas)
        {
            this.idFavorita = favoritas.idFavorita;
            this.idUsuario = favoritas.idUsuario;
            this.fecharAnadidaFavorita = favoritas.fecharAnadidaFavorita;
            this.Usuario = favoritas.Usuario;
            this.idCancion = favoritas.idCancion;
            this.Cancion = favoritas.Cancion;
        }       
        public CancionesFavoritasViewModel() { }
        public static List<CancionesFavoritasViewModel> ListarFavoritasPorUsuario(int idUsuario)
        {
            var favoritas = CancionesManager.ListarFavoritasPorUsuario(idUsuario);
            List<CancionesFavoritasViewModel> lista = new List<CancionesFavoritasViewModel>();
            foreach (var item in favoritas)
            {
                CancionesFavoritasViewModel model = new CancionesFavoritasViewModel(item);
                lista.Add(model);
            }
            return lista;
        }
    }
}
