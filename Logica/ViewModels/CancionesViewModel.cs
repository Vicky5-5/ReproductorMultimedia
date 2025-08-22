using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logica.Managers;
using Logica.Models;
using Microsoft.AspNetCore.Http;

namespace Logica.ViewModels
{
    public class CancionesViewModel
    {
        public int idCancion { get; set; }
        public string Titulo { get; set; }
        public string Artista { get; set; }
        public string Album { get; set; }
        public int Year { get; set; }

        [RegularExpression(@"^([0-5]?\d):([0-5]\d)$", ErrorMessage = "Duración debe tener formato mm:ss")]
        public TimeSpan Duracion { get; set; }
        [NotMapped]
        [RegularExpression(@"^([0-5]?\d):([0-5]\d)$", ErrorMessage = "Duración debe tener formato mm:ss")]
        public string DuracionTexto { get; set; } // para la vista y validación
        public int NumeroReproducciones { get; set; } = 0;
        public int NumeroLikes { get; set; } = 0;
        [NotMapped]
        public IFormFile ArchivoCancion { get; set; }
        public string RutaArchivo { get; set; } // Ruta del archivo
        public string RutaCaratulaAlbum { get; set; }
        public bool UsuarioDioLike { get; set; }=false;

        public IFormFile CaratulaAlbum { get; set; }
        public Genero Genero { get; set; }

        public virtual ICollection<ListaReproduccion> ListaReproduccion { get; set; }
        public CancionesViewModel(Canciones canciones)
        {
            this.idCancion = canciones.idCancion;
            this.Titulo = canciones.Titulo;
            this.Artista = canciones.Artista;
            this.Album = canciones.Album;
            this.Duracion = canciones.Duracion;
            this.DuracionTexto = canciones.Duracion.ToString(@"m\:ss");
            this.NumeroReproducciones = canciones.NumeroReproducciones;
            this.NumeroLikes = canciones.NumeroLikes;
            this.ArchivoCancion = canciones.ArchivoCancion;
            this.RutaArchivo = canciones.RutaArchivo;
            this.Genero = canciones.Genero;
            this.Year = canciones.Year;
            this.RutaCaratulaAlbum = canciones.RutaCaratulaAlbum;
            this.CaratulaAlbum = canciones.CaratulaAlbum;
            this.UsuarioDioLike = canciones.UsuarioDioLike;
        }        
        public CancionesViewModel()
        {
        }
        public static CancionesViewModel ObtenerCancionView(int id)
        {
            //Se guarda el producto de la base de datos, del objeto producto y se retorna el producto entero
            var nuevo = CancionesManager.ObtenerCancion(id);
            CancionesViewModel model = new CancionesViewModel(nuevo);

            return model;
        }
        //List
        public static List<CancionesViewModel> ListSongs()
        {

            var listar = CancionesManager.ListarCanciones();
            List<CancionesViewModel> lista = new List<CancionesViewModel>();
            foreach (var item in listar)
            {
                CancionesViewModel model = new CancionesViewModel(item);

                lista.Add(model);

            }
            return lista;

        }
        public static CancionesViewModel DatosUnProducto(int id)
        {

            var listar = CancionesManager.ObtenerDatosUnaCancion(id);

            CancionesViewModel model = new CancionesViewModel(listar);
            return model;

        }

        public static List<CancionesViewModel> ListarFavoritasComoCanciones(int? idUsuario)
        {
            return CancionesManager.ListSongsConLikes(idUsuario);

        }

        public static CancionesViewModel AddSong(int id, string titulo, string artista, string album, TimeSpan duracion, int reproducciones, int likes, string ruta, IFormFile cancion,Genero genero, int year, IFormFile caratula, string rutaCaratula)
        {
            var guardado = CancionesManager.GuardarCancion(id, titulo, artista, album, duracion, reproducciones, likes, ruta, cancion,genero,year,caratula,rutaCaratula);

            CancionesViewModel model = new CancionesViewModel(guardado);
            return model;
        }

        public static void RemoveCancion(int id)
        {
            CancionesManager.EliminarCancion(id);
        }

        public static void UpdateSong(int id)
        {
            CancionesManager.ActualizarReproducciones(id);
        }
        public static int UpdateLikes(int id)
        {
            var likes=CancionesManager.ActualizarLikes(id);
            return likes;
        }
        public static bool LikeDislike(int idUsuario, int idCancion)
        {
            if (idCancion != 0 && idUsuario != 0)
            {
                return CancionesManager.AlternarLike(idUsuario, idCancion);
            }
            return false;
        }

        //public static List<CancionesViewModel> ListarFavoritasPorUsuario(int idUsuario)
        //{
        //    var favoritas = CancionesManager.ListarFavoritasPorUsuario(idUsuario);
        //    List<CancionesViewModel> lista = new List<CancionesViewModel>();
        //    foreach (var item in favoritas)
        //    {
        //        CancionesViewModel model = new CancionesViewModel(item);
        //        lista.Add(model);
        //    }
        //    return lista;
        //}
    }
}
