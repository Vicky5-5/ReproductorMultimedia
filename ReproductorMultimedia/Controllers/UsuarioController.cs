using Logica.Models;
using Logica.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReproductorMultimedia.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: UsuarioController
        public ActionResult Administrador()
        {
            List<UsuarioViewModel> lista = new List<UsuarioViewModel>();

            lista = UsuarioViewModel.ListUsuarios();
            return View(lista);
        }

        // GET: UsuarioController/Details/5
        public ActionResult Details(int id)
        {
            var prViewModel = UsuarioViewModel.DatosUnUsuario(id);

            if (prViewModel == null)
            {
                //return HttpNotFound();
            }
            return View(prViewModel);
        }

        // GET: UsuarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsuarioViewModel usu)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var model = UsuarioViewModel.AddUsuario(usu.idUsuario, usu.Nombre, usu.Email, usu.Password, usu.Estado, usu.fechaBaja, usu.Administrador);
                    return RedirectToAction("Administrador");

                    //EnviarCorreoBienvenida(model.Email);
                }
                catch (Exception ex)
                {

                    ViewBag.Error = $"Error al guardar: {ex.Message}";
                }
            }

            return View(usu);
        }

        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {
            UsuarioViewModel prViewModel = UsuarioViewModel.GetUsuario(id);

            if (prViewModel == null)
            {
                //return HttpNotFound();
            }
            return View(prViewModel);
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsuarioViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var model = UsuarioViewModel.AddUsuario(usuario.idUsuario, usuario.Nombre, usuario.Email, usuario.Password, usuario.Estado, usuario.fechaBaja, usuario.Administrador);

                    //Si el usuario se da de baja que envíe otro correo
                    if (model.Estado == false)
                    {
                        // EnviarCorreoBaja(model.Email);
                        return RedirectToAction("Administrador");

                    }
                    else
                    {
                       // EnviarCorreoEditado(model.Email);
                        return RedirectToAction("Administrador");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"Error al guardar: {ex.Message}";
                }
            }

            return View(usuario);
        }

        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            var prViewModel = UsuarioViewModel.GetUsuario(id);

            if (prViewModel == null)
            {
                //return HttpNotFound();
            }
            return View(prViewModel);
        }

        // POST: UsuarioController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    UsuarioViewModel.RemoveUsuario(id);
                    return View("VistaUsuarios");

                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"Error al guardar: {ex.Message}";

                }
            }
            return View("Index");
        }
    }
}
