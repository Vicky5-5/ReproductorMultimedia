using Logica.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReproductorMultimedia.Controllers
{
    public class CancionesController : Controller
    {
        // GET: CancionesController
        public ActionResult ListarCanciones()
        {
            List<CancionesViewModel> lista = new List<CancionesViewModel>();

            lista = CancionesViewModel.ListProductos();
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarCancion(IFormCollection collection)
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
