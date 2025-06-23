using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logica.Managers;
using Logica.Models;

namespace Logica.ViewModels
{
    public class UsuarioViewModel
    {
        public int idUsuario { get; set; }

        public string Nombre { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public bool Estado { get; set; }

        public DateTime fechaRegistro { get; set; }
        public DateTime? fechaBaja { get; set; }
        public bool Administrador { get; set; }
        public virtual ICollection<ListaReproduccion> ListasReproduccion { get; set; }

        public UsuarioViewModel(Usuario usuario)
        {
            this.idUsuario = usuario.idUsuario;
            this.Nombre = usuario.Nombre;
            this.Email = usuario.Email;
            this.Password = usuario.Password;
            this.fechaBaja = usuario.fechaBaja;
            this.Administrador = usuario.Administrador;
        }
        public UsuarioViewModel(int id, string nombre, string email, string password, string direccion)
        {
            this.idUsuario = id;
            this.Nombre = Nombre;
            this.Email = Email;
            this.Password = Password;
            this.fechaBaja = null;
            this.Administrador = false;
        }
        public UsuarioViewModel()
        {

        }

        #region Métodos

        public static UsuarioViewModel GetUsuario(int id)
        {
            //Se guarda el producto de la base de datos, del objeto producto y se retorna el producto entero
            var model = UsuariosManager.ObtenerUsuario(id);

            UsuariosManager usuario = new UsuariosManager(model);

            return usuario;
        }
        public static List<UsuariosManager> ListUsuarios()
        {

            var listar = UsuariosManager.ListarUsuarios();
            List<UsuarioViewModel> lista = new List<UsuarioViewModel>();
            foreach (var item in listar)
            {
                UsuarioViewModel model = new UsuarioViewModel(item);

                lista.Add(model);

            }
            return lista;

        }

        public static UsuarioViewModel DatosUnUsuario(int id)
        {

            var listar = UsuariosManager.ObtenerDatosUnUsuario(id);

            UsuarioViewModel model = new UsuarioViewModel(listar);

            return model;

        }
        //Es usado por el modo editar y crear
        public static UsuarioViewModel AddUsuario(int id, string nombre, string email, string password, bool estado, DateTime fechaRegistro, DateTime? fechaBaja, string direccion, bool administrador)
        {
            var guardado = UsuariosManager.GuardarUsuario(id, nombre, email, password, estado, fechaRegistro, fechaBaja, direccion, administrador);

            UsuarioViewModel model = new UsuarioViewModel(guardado);


            return model;
        }
        public static UsuarioViewModel RemoveUsuario(int id)
        {
            var borrado = UsuariosManager.EliminarUsuario(id);

            UsuarioViewModel model = new UsuarioViewModel(borrado);

            return model;
        }
        public UsuarioViewModel LoginViewModel(string email, string password)
        {

            var login = UsuariosManager.Login(email, password);
            UsuarioViewModel model = new UsuarioViewModel(login);

            return model;
        }

        public static UsuarioViewModel RegistroUsuarioNuevo(int id, string nombre, string email, string password, string direccion)
        {
            var registro = UsuariosManager.RegistrarUsuario(id, nombre, email, password, direccion);
            UsuarioViewModel model = new UsuarioViewModel(id, nombre, email, password, direccion);

            return model;
        }
    }
}
