using Logica.Models;
using Logica.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net.Mail;
using System.Text;

namespace ReproductorMultimedia.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly CorreoService _correoService;
        public UsuarioController(CorreoService correoService)
        {
            _correoService = correoService;
        }

        // GET: UsuarioController
        public ActionResult Administrador()
        {
            var nombreUsuario = HttpContext.Session.GetString("Nombre");
            ViewBag.NombreUsuario = nombreUsuario;

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
                    _correoService.EnviarCorreoAlta(model.Email);

                    return RedirectToAction("Administrador");

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
            var usuario = UsuarioViewModel.GetUsuario(id);
            if (usuario == null)
                return NotFound();

            return View(usuario);
        }


        // POST: UsuarioController/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(UsuarioViewModel model)
        {
            if (model.idUsuario == 0)
            {
                // id nulo o no recibido
                return BadRequest("ID de usuario no válido.");
            }

            UsuarioViewModel.RemoveUsuario(model.idUsuario);
            return RedirectToAction("Administrador");
        }


    }
}
