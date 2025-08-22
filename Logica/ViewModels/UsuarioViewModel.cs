using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Email no válido")]
        public string Email { get; set; }

        [StringLength(4000, MinimumLength = 10, ErrorMessage = "La contraseña debe tener al menos 10 caracteres")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool Estado { get; set; }

        [DataType(DataType.Date)]
        public DateTime? fechaBaja { get; set; }
        public DateTime fechaRegistro { get; set; } = DateTime.Now;

        public bool Administrador { get; set; }
        [NotMapped]
        public virtual ICollection<ListaReproduccion> ListasReproduccion { get; set; }

        public UsuarioViewModel(Usuario usuario)
        {
            this.idUsuario = usuario.idUsuario;
            this.Nombre = usuario.Nombre;
            this.Email = usuario.Email;
            this.Password = usuario.Password;
            this.fechaBaja = usuario.fechaBaja;
            this.Administrador = usuario.Administrador;
            this.fechaRegistro = DateTime.Now; 
        }
        //PARA REGISTAR A UN USUARIO
        public UsuarioViewModel(int id, string nombre, string email, string password)
        {
            this.idUsuario = id;
            this.Nombre = nombre;
            this.Email = email;
            this.Password = password;
            this.fechaBaja = null;
            this.Administrador = false;
        }

        /// <summary>
        /// Hay que iniciarla para evitar las referencias nulas ya que podríamos intentar agregar elementos y así poder interactuar con ella.
        /// 
        /// Al inicializarla nos garantizamos que cada instacia de la clase tenga consistencia.Esto es especialmente importante si en algún momento 
        /// se decide agregar listas de reproducción al usuario, 
        /// ya que siempre tendrás una colección lista para recibir esos elementos.
        /// 
        /// Si la colección está inicializada, puedes agregar, eliminar o manipular elementos de ListasReproduccion sin tener que verificar si la colección es nula. 
        /// Esto simplifica el código y reduce la posibilidad de errores.
        /// 
        /// Si en el futuro se decide guardar las listas de reproducción asociadas a un usuario en la base de datos, tener la colección inicializada 
        /// permitirá trabajar con ella de manera más sencilla.
        /// 
        /// Tener la colección inicializada puede facilitar este proceso, ya que no tendrás que manejar casos donde la colección sea nula.
        /// </summary>
        public UsuarioViewModel()
        {
            ListasReproduccion = new List<ListaReproduccion>(); // Inicialización
        }

        #region Métodos

        public static UsuarioViewModel GetUsuario(int id)
        {
            //Se guarda el producto de la base de datos, del objeto producto y se retorna el producto entero
            var model = UsuariosManager.ObtenerUsuario(id);

            UsuarioViewModel usuario = new UsuarioViewModel(model);

            return usuario;
        }
        public static List<UsuarioViewModel> ListUsuarios()
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
        public static UsuarioViewModel AddUsuario(int id, string nombre, string email, string password, bool estado, DateTime? fechaBaja, bool administrador)
        {
            var guardado = UsuariosManager.GuardarUsuario(id, nombre, email, password, estado, fechaBaja, administrador);

            UsuarioViewModel model = new UsuarioViewModel(guardado);


            return model;
        }
        public static void RemoveUsuario(int id)
        {
            UsuariosManager.EliminarUsuario(id);
        }
        public UsuarioViewModel LoginViewModel(string email, string password)
        {

            var login = UsuariosManager.Login(email, password);
            UsuarioViewModel model = new UsuarioViewModel(login);

            return model;
        }

        public static UsuarioViewModel RegistroUsuarioNuevo(int id, string nombre, string email, string password)
        {
            var registro = UsuariosManager.RegistrarUsuario(id, nombre, email, password);
            UsuarioViewModel model = new UsuarioViewModel(id, nombre, email, password);

            return model;
        }

        public static void BajaVoluntaria(int idUsuario)
        {
           UsuariosManager.DadaDeBaja(idUsuario);
        }
    }
}
#endregion