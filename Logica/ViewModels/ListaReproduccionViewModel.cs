using Logica.Managers;
using Logica.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.ViewModels
{

    public class ListaReproduccionViewModel
    {
        public Guid idLista { get; set; }

        [Required(ErrorMessage = "El nombre de la lista es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string NombreLista { get; set; }

        public DateTime fechaCreacion { get; set; }

        public DateTime cancionAnadida { get; set; }

        public CancionesViewModel oCanciones { get; set; }
        public UsuarioViewModel oUsuario{ get; set; }
       
        public List<CancionesViewModel> Canciones { get; set; } = new List<CancionesViewModel>();

        public ListaReproduccionViewModel() { }

        public ListaReproduccionViewModel(ListaReproduccion lista)
        {
            oCanciones = new CancionesViewModel();
            oUsuario = new UsuarioViewModel();

            idLista = lista.idLista;
            NombreLista = lista.NombreLista;
            fechaCreacion = lista.fechaCreacion;
            cancionAnadida = lista.cancionAnadida;
            oCanciones.idCancion = lista.idCancion;
            oUsuario.idUsuario = lista.idUsuario;

            if (lista.Cancion != null)
            {
                oCanciones.Titulo = lista.Cancion.Titulo;
                oCanciones.Artista = lista.Cancion.Artista;
                oCanciones.Album = lista.Cancion.Album;
                oCanciones.RutaArchivo = lista.Cancion.RutaArchivo;
                oCanciones.RutaCaratulaAlbum = lista.Cancion.RutaCaratulaAlbum;
                oCanciones.Duracion = lista.Cancion.Duracion;
                oCanciones.NumeroLikes = lista.Cancion.NumeroLikes;
            }

            if (lista.Usuario != null)
            {
                oUsuario.Nombre= lista.Usuario.Nombre;
            }
        }

        public static ListaReproduccionViewModel CrearLista(int idUsuario, string nombreLista, List<int> idsCanciones)
        {
            var lista = ListasReproduccionManager.CrearListaPersonalizada(idUsuario, nombreLista, idsCanciones);

            if (lista == null)
                return null;

            return new ListaReproduccionViewModel(lista);
        }


        public static List<ListaReproduccionViewModel> ObtenerListasPorUsuario(int idUsuario)
        {
            var listas = ListasReproduccionManager.ObtenerTodasLasListasPorUsuario(idUsuario);

            List<ListaReproduccionViewModel> listasVM = new List<ListaReproduccionViewModel>();
            foreach (var lista in listas)
            {
             ListaReproduccionViewModel model = new ListaReproduccionViewModel(lista);
                listasVM.Add(model);
            }
            return listasVM;
        }
        public static void BorrarLista(int idUsuario, Guid idLista)
        {
            ListasReproduccionManager.BorrarLista(idUsuario, idLista);
        }




    }
}
