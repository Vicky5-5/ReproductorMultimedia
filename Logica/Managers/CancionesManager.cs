﻿using System;
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

        public static Canciones GuardarCancion(int id, string titulo, string artista, string album, TimeSpan duracion, int reproducciones, int likes, string ruta, IFormFile cancion, Genero genero, int year, IFormFile caratula, string rutaCaratula)
        {
            using (var db = new Conexion())
            {
                //Comprobamos si existe
                var canciones = db.Canciones.FirstOrDefault(a => a.idCancion == id);

                if (canciones != null)
                {
                    canciones.Titulo = titulo;
                    canciones.Artista = artista;
                    canciones.Album = album;
                    canciones.Duracion = duracion;
                    canciones.NumeroReproducciones = reproducciones;
                    canciones.NumeroLikes = likes;
                    canciones.RutaArchivo = ruta;
                    canciones.Genero = genero;
                    canciones.Year = year;
                    canciones.RutaCaratulaAlbum = rutaCaratula;
                    db.SaveChanges();
                    return canciones;
                }
                //Si no existe, se crea una instancia con reproducciones, likes y génnero iniciales
                canciones = new Canciones
                {
                    NumeroReproducciones = reproducciones,
                    NumeroLikes = likes,
                    Genero = genero
                };


                try
                {
                    // Guardamos el archivo MP3 en el proyecto
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CancionesAgregadas");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    if (cancion == null || cancion.Length == 0)
                        throw new ArgumentException("No se ha proporcionado ningún archivo de audio válido.");

                    // Nombre único del archivo
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(cancion.FileName);
                    var fullPath = Path.Combine(uploadsFolder, fileName);

                    // Guardar archivo
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        cancion.CopyTo(fileStream);
                    }

                    // Leemos los metadatos del archivo, si el metadato no existe utiliza el valor introducido en el formulario
                    var file = TagLib.File.Create(fullPath);
                    canciones.Titulo = !string.IsNullOrWhiteSpace(file.Tag.Title) ? file.Tag.Title : titulo;
                    canciones.Artista = file.Tag.Performers != null && file.Tag.Performers.Length > 0
                        ? string.Join(", ", file.Tag.Performers)
                        : artista;
                    canciones.Album = file.Tag.Album ?? album;
                    var duracionCompleta = file.Properties.Duration;
                    canciones.Duracion = new TimeSpan(0, duracionCompleta.Minutes, duracionCompleta.Seconds);

                    // Guardamos la ruta
                    canciones.RutaArchivo = "/CancionesAgregadas/" + fileName;
                    canciones.RutaCaratulaAlbum = rutaCaratula;
                    // Guardamos en base de datos
                    db.Canciones.Add(canciones);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    throw new Exception("Error al guardar canción: " + ex.Message);
                }

                return canciones;
            }
        }

        public static void EliminarCancion(int id)
        {
            using (var db = new Conexion())
            {
                var cancion = db.Canciones.FirstOrDefault(a => a.idCancion == id);
                var eliminado = db.Canciones.Remove(cancion);
                db.SaveChanges();
            }
        }
        public static void ActualizarReproducciones(int id)
        {
            using (var db = new Conexion())
            {
                var cancion = db.Canciones.FirstOrDefault(a => a.idCancion == id);
                if (cancion != null)
                {
                    cancion.NumeroReproducciones++;
                    db.SaveChanges();
                }
            }
        }

        public static bool AlternarLike(int idUsuario, int idCancion)
        {
            using (var db = new Conexion())
            {
                // Vemos si el usuario ya ha dado like a la canción
                var favoritoExistente = db.Favoritas
                    .FirstOrDefault(cf => cf.idUsuario == idUsuario && cf.idCancion == idCancion);

                var cancion = db.Canciones.FirstOrDefault(c => c.idCancion == idCancion);

                if (cancion == null)
                    return false; // Canción no encontrada

                if (favoritoExistente != null)
                {
                    // Si ya dio like → eliminar favorito
                    db.Favoritas.Remove(favoritoExistente);

                    if (cancion.NumeroLikes > 0)
                        cancion.NumeroLikes--;
                    return false; // Like eliminado
                }
                else
                {
                    // No dio like → añadir favorito
                    var nuevoFavorito = new CancionesFavoritas
                    {
                        idUsuario = idUsuario,
                        idCancion = idCancion,
                        fecharAnadidaFavorita = DateTime.Now
                    };

                    db.Favoritas.Add(nuevoFavorito);
                    cancion.NumeroLikes++;
                }

                db.SaveChanges();
                return true; // Like añadido
            }
        }

        public static int ActualizarLikes(int id)
        {
            using (var db = new Conexion())
            {
                var cancion = db.Canciones.FirstOrDefault(c => c.idCancion == id);
                if (cancion != null)
                {
                    cancion.NumeroLikes++;
                    db.SaveChanges();
                    return cancion.NumeroLikes;
                }

                return -1; // Devuelve -1 si la canción no se encuentra
            }
        }


    }
}
