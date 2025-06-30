using Grpc.Core;
using Logica.Managers;
using Logica.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReproductorMultimedia.Controllers
{
    public class CancionesController : Controller
    {
        public CancionesController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        // GET: CancionesController
        public ActionResult ListaCanciones()
        {
            List<CancionesViewModel> lista = new List<CancionesViewModel>();

            lista = CancionesViewModel.ListSongs();
            //lista.AddRange(viewModel.ListProductos()); //Esto es otra forma de hacer el listado
            return View(lista);
        }

        // GET: CancionesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CancionesController/Create
        public ActionResult AgregarCancion()
        {
            return View();
        }

        // POST: CancionesController/Create
     

    
        private readonly IWebHostEnvironment _hostingEnvironment;


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarCancion(CancionesViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // Validar duplicados (idealmente esta validación también debería estar en el Manager)
                var existeCancion = CancionesManager.ListarCanciones()
                    .Any(c => c.Titulo.Equals(model.Titulo, StringComparison.OrdinalIgnoreCase) &&
                              c.Artista.Equals(model.Artista, StringComparison.OrdinalIgnoreCase));

                if (existeCancion)
                {
                    ModelState.AddModelError("", "Ya existe una canción con el mismo título y artista.");
                    return View(model);
                }

                if (model.ArchivoCancion != null && model.ArchivoCancion.Length > 0)
                {
                    string extension = Path.GetExtension(model.ArchivoCancion.FileName);
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;

                    string relativo = "/Canciones/" + fileName;
                    string absoluto = Path.Combine(_hostingEnvironment.WebRootPath, "Canciones", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(absoluto)!);

                    using (var fileStream = new FileStream(absoluto, FileMode.Create))
                    {
                        model.ArchivoCancion.CopyTo(fileStream);
                    }

                    model.RutaArchivo = relativo;

                    // Opcional: obtener duración con TagLib si quieres
                    var tagFile = TagLib.File.Create(absoluto);
                    model.Duracion = tagFile.Properties.Duration;
                }

                // Aquí sólo llamas al Manager para guardar, sin lógica de DB en el controlador
                CancionesManager.GuardarCancion(
                    model.idCancion,
                    model.Titulo,
                    model.Artista,
                    model.Album,
                    model.Duracion,
                    model.NumeroReproducciones,
                    model.NumeroLikes,
                    model.RutaArchivo,
                    model.ArchivoCancion
                );

                return RedirectToAction("ListaCanciones");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al guardar: {ex.Message}";
                return View(model);
            }
        }




        // GET: CancionesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CancionesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CancionesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CancionesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
