using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logica.Contexto;
using Logica.Models;
using Microsoft.AspNetCore.Http;

namespace Logica.Managers
{
    public class CancionesManager
    {
        public static Canciones ObtenerCancion(int id)
        {
            using (Conexion db = new Conexion())
            {

                //Obtener el id del canción desde la base de datos
                var cancion = db.Canciones.FirstOrDefault(a => a.idCancion == id);
                return cancion;
            }
        }
        public static Canciones ObtenerDatosUnaCancion(int id)
        {
            using (var db = new Conexion())
            {

                var producto = db.Canciones.SingleOrDefault(p => p.idCancion == id);
                return producto;
            }
        }
        public static List<Canciones> ListarCanciones()
        {
            using (var db = new Conexion())
            {
                List<Canciones> productos = db.Canciones.ToList();
                return productos;
            }
        }

        public static Canciones GuardarCancion(int id, string titulo, string artista, string album, TimeSpan duracion, int reproducciones, int likes, string ruta, IFormFile cancion) //Esto sirve para editar y crear
        {
            using (var db = new Conexion())
            {

                var canciones = db.Canciones.FirstOrDefault(a => a.idCancion == id);
                //Si el id es distinto de entramos en editar
                if (canciones != null)
                {
                    canciones.idCancion = id;
                    canciones.Titulo = titulo;
                    canciones.Artista = artista;
                    canciones.Album = album;
                    canciones.Duracion = duracion;
                    canciones.NumeroReproducciones = reproducciones;
                    canciones.NumeroLikes = likes;
                    canciones.RutaArchivo = ruta;

                    db.SaveChanges();
                    return canciones;
                }

                //Esto es para crear un nuevo producto
                canciones = new Canciones()
                {

                    idCancion = id,
                    Titulo = titulo,
                    Artista = artista,
                    Album = album,
                    Duracion = duracion,
                    NumeroReproducciones = reproducciones,
                    NumeroLikes = likes,
                    RutaArchivo = ruta,


                };

                try
                {
                    // Guardar el archivo en el servidor
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CancionesAgregadas");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);
                    var fullPath = Path.Combine(uploadsFolder, ruta); using (var fileStream = new FileStream(uploadsFolder, FileMode.Create))
                    {
                        cancion.CopyTo(fileStream);
                    }
                    // Leer los metadatos de la canción
                    var file = TagLib.File.Create(uploadsFolder);
                    canciones.Titulo = file.Tag.Title ?? canciones.Titulo; // Título de la canción
                    canciones.Artista = string.Join(", ", file.Tag.Performers) ?? canciones.Artista; // Artista(s)
                    canciones.Album = file.Tag.Album ?? canciones.Album; // Álbum
                    canciones.Duracion = file.Properties.Duration; // Duración
                    db.Canciones.Add(canciones);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    throw new Exception(ex.Message);
                }
                return canciones;
            }

        }

        public static void EliminarCancion(int id)
        {
            using (var db = new Conexion())
            {
                var producto = db.Canciones.FirstOrDefault(a => a.idCancion == id);
                var eliminado = db.Canciones.Remove(producto);
                db.SaveChanges();
            }
        }
    }
}
