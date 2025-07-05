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
        public IActionResult AgregarCancion(int idCancion,string titulo, string artista, string album,string DuracionTexto,IFormFile archivo, string ruta,Genero genero)
        {
            if (!TimeSpan.TryParseExact(DuracionTexto, @"m\:ss", null, out var duracion))
            {
                ModelState.AddModelError("DuracionTexto", "Formato inválido, usa mm:ss");
                return View();
            }

            if (archivo == null || archivo.Length == 0)
            {
                ModelState.AddModelError("ArchivoCancion", "Debes subir un archivo MP3.");
                return View();
            }

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CancionesAgregadas");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(archivo.FileName)}";
            var fullPath = Path.Combine(folderPath, uniqueFileName);
            var relativePath = $"/CancionesAgregadas/{uniqueFileName}";

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                archivo.CopyTo(stream);
            }

            CancionesManager.GuardarCancion(
                idCancion,
                titulo,
                artista,
                album,
                duracion,
                0, 0,
                relativePath,
                null, // no se guarda el archivo binario
                genero
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
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: Canciones/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            return RedirectToAction("Index");
        }
    }
}
