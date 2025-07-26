using Logica.Managers;
using Logica.Models;
using Logica.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReproductorMultimedia.Controllers
{
    public class CancionesController : Controller
    {
        // GET: Canciones/ListaCanciones
        public IActionResult ListaCanciones()
        {
            var lista = CancionesViewModel.ListSongs();
            return View(lista);
        }

        // GET: Canciones/AgregarCancion
        public IActionResult AgregarCancion()
        {
            return View();
        }

        // POST: Canciones/AgregarCancion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarCancion(int idCancion,string titulo, string artista, string album,string DuracionTexto,IFormFile archivo, Genero genero, int year, IFormFile caratula, string rutaCaratula)
        {
            //Convertimos el texto en TimeSpan y si el formato es incorrecto nos notifica
            if (!TimeSpan.TryParseExact(DuracionTexto, @"m\:ss", null, out var duracion))
            {
                ModelState.AddModelError("DuracionTexto", "Formato inválido, usa mm:ss");
                return View();
            }
            //Comporbamos que el archivo existe
            if (archivo == null || archivo.Length == 0)
            {
                ModelState.AddModelError("ArchivoCancion", "Debes subir un archivo MP3.");
                return View();
            }
            //Preparamos la carpeta para guardar la canción
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CancionesAgregadas");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            //Generamos un nombre aleatoria único para evitar conflictos

            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(archivo.FileName)}";
            var fullPath = Path.Combine(folderPath, uniqueFileName);
            var relativePath = $"/CancionesAgregadas/{uniqueFileName}";

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                archivo.CopyTo(stream);
            }
            var caratulaFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Caratulas");
            if (!Directory.Exists(caratulaFolder))
                Directory.CreateDirectory(caratulaFolder);

            var caratulaFileName = $"{Guid.NewGuid()}{Path.GetExtension(caratula.FileName)}";
            var caratulaFullPath = Path.Combine(caratulaFolder, caratulaFileName);
            var caratulaRelativePath = $"/Caratulas/{caratulaFileName}";

            using (var stream = new FileStream(caratulaFullPath, FileMode.Create))
            {
                caratula.CopyTo(stream);
            }

            CancionesViewModel.AddSong(
                idCancion,
                titulo,
                artista,
                album,
                duracion,
                0, 0,
                relativePath,
                archivo, // no se guarda el archivo binario
                genero,
                year,
                caratula,
                caratulaRelativePath
            );

            return RedirectToAction("ListaCanciones");
        }


        // GET: Canciones/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: Canciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            return RedirectToAction("Index");
        }

        // GET: Canciones/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var cancion = CancionesViewModel.ObtenerCancionView(id);
            if (cancion == null)
            {
                return NotFound();
            }
            return View(cancion);
        }

        // POST: Canciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // RemoveCancion is a void method, so no assignment is needed.
                    CancionesViewModel.RemoveCancion(id);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"Error al guardar: {ex.Message}";
                }
            }
            return RedirectToAction("ListaCanciones");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarReproducciones(int id)
        {
            try
            {
                CancionesViewModel.UpdateSong(id);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }


    }
}
