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

        [Range(typeof(TimeSpan), "00:00:00", "01:00:00", ErrorMessage = "La duración tiene que estar entre 0 y 30 minutos")]
        public TimeSpan Duracion { get; set; }
        public int NumeroReproducciones { get; set; } = 0;
        public int NumeroLikes { get; set; } = 0;
        [NotMapped]
        public IFormFile ArchivoCancion { get; set; }
        public string RutaArchivo { get; set; } // Ruta del archivo

        public virtual ICollection<ListaReproduccion> ListaReproduccion { get; set; }
        public CancionesViewModel(Canciones canciones)
        {
            this.idCancion = canciones.idCancion;
            this.Titulo = canciones.Titulo;
            this.Artista = canciones.Artista;
            this.Album = canciones.Album;
            this.Duracion = canciones.Duracion;
            this.NumeroReproducciones = canciones.NumeroReproducciones;
            this.NumeroLikes = canciones.NumeroLikes;
            this.ArchivoCancion = canciones.ArchivoCancion;
            this.RutaArchivo = canciones.RutaArchivo;
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
        public static List<CancionesViewModel> ListProductos()
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

        public static CancionesViewModel AddProducto(int id, string titulo, string artista, string album, TimeSpan duracion, int reproducciones, int likes, string ruta, IFormFile cancion)
        {
            var guardado = CancionesManager.GuardarProducto(id, titulo, artista, album, duracion, reproducciones, likes, ruta, cancion);

            CancionesViewModel model = new CancionesViewModel(guardado);
            return model;
        }

        public static void RemoveProducto(int id)
        {
            CancionesManager.EliminarCancion(id);
        }

    

    }
}
