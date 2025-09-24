using Logica.Contexto;
using Logica.Models;
using Logica.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Logica.Managers
{
    public class ListasReproduccionManager
    {
        public static ListaReproduccion CrearListaPersonalizada(int idUsuario, string nombreLista, List<int> idsCanciones)
        {
            using (var db = new Conexion())
            {
                var idLista = Guid.NewGuid();
                var fechaCreacion = DateTime.Now;

                // Validación defensiva
                if (string.IsNullOrWhiteSpace(nombreLista))
                    throw new ArgumentException("El nombre de la lista no puede estar vacío.");

                if (idsCanciones == null || !idsCanciones.Any())
                    throw new ArgumentException("Debes seleccionar al menos una canción.");

                foreach (var idCancion in idsCanciones)
                {
                    var cancion = db.Canciones.FirstOrDefault(c => c.idCancion == idCancion);
                    if (cancion == null) continue;

                    var entradaLista = new ListaReproduccion
                    {
                        idLista = idLista,
                        NombreLista = nombreLista,
                        fechaCreacion = fechaCreacion,
                        cancionAnadida = DateTime.Now,
                        idUsuario = idUsuario,
                        idCancion = idCancion
                    };

                    db.ListaReproduccion.Add(entradaLista);
                }

                db.SaveChanges();

                // Recuperamos una entrada representativa de la lista
                return db.ListaReproduccion
                         .Include(l => l.Cancion)
                         .Include(l => l.Usuario)
                         .FirstOrDefault(l => l.idLista == idLista);
            }
        }


        public static List<ListaReproduccion> ObtenerTodasLasListasPorUsuario(int idUsuario)
        {
            using (var db = new Conexion())
            {
                return db.ListaReproduccion
                         .Where(l => l.idUsuario == idUsuario)
                         .Select(l => new ListaReproduccion
                         {
                             idLista = l.idLista,
                             NombreLista = l.NombreLista,
                             fechaCreacion = l.fechaCreacion
                         })
                         .ToList();
            }
        }
        public static List<Canciones> ObtenerCancionesPorLista(Guid idLista, int idUsuario)
        {
            using (var db = new Conexion())
            {
                return db.ListaReproduccion
                         .Where(l => l.idLista == idLista && l.idUsuario == idUsuario)
                         .Select(l => l.Cancion)
                         .ToList();
            }
        }

        public static String ObtenerNombreLista(Guid idLista, int idUsuario)
        {
            using (var db = new Conexion())
            {
                var lista = db.ListaReproduccion
                              .FirstOrDefault(l => l.idLista == idLista && l.idUsuario == idUsuario);
                return lista?.NombreLista;
            }
        }


        public static void BorrarLista(int idUsuario, Guid idLista)
        {
            using (var db = new Conexion())
            {
                //Obtenemos las entradas de la lista
                var entradasLista = db.ListaReproduccion
                                      .Where(l => l.idUsuario == idUsuario && l.idLista == idLista)
                                      .ToList();
                //Borramos las canciones de la lista
                if (!entradasLista.Any())
                    throw new InvalidOperationException("La lista no existe o no pertenece al usuario.");
                db.ListaReproduccion.RemoveRange(entradasLista);
                db.SaveChanges();
            }
        }
    }
}
