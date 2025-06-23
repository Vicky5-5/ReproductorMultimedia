using System.Data.Entity.Validation;
using System.Security.Cryptography;
using System.Text;
using Logica.Contexto;
using Logica.Models;
namespace Logica.Managers
{
    public class UsuariosManager
    {
        public static Usuario ObtenerUsuario(int id)
        {
            using (Conexion db = new Conexion())
            {

                //Obtener el id del producto desde la base de datos
                Usuario usuario = db.Usuarios.FirstOrDefault(a => a.idUsuario == id);
                return usuario;
            }
        }
        public static Usuario ObtenerDatosUnUsuario(int id)
        {
            using (var db = new Conexion())
            {

                var usuario = db.Usuarios.SingleOrDefault(p => p.idUsuario == id);

                return usuario;
            }
        }
        public static Usuario GuardarUsuario(int id, string nombre, string email, string password, bool estado, DateTime fechaRegistro, DateTime? fechaBaja, string direccion, bool administrador) //Esto sirve para editar y crear
        {
            using (var db = new Conexion())
            {

                var usuario = db.Usuarios.FirstOrDefault(a => a.idUsuario == id);
                var pEncript = HashPassword(password);
                //Productos productos = new Productos();
                //var producto = productos.ObtenerProducto(id);
                //Si el id es distinto de entramos en editar
                if (usuario != null)
                {
                    usuario.idUsuario = id;
                    usuario.Nombre = nombre;
                    usuario.Email = email;
                    usuario.Password = pEncript;
                    usuario.Estado = estado;
                    usuario.fechaBaja = fechaBaja;
                    usuario.Administrador = administrador;
                    db.SaveChanges();
                    return usuario;
                }

                //Esto es para crear un nuevo producto
                //Crear una prueba de error de, si existe el email, que lo eche para atrás la creación
                usuario = new Usuario()
                {

                    idUsuario = id,
                    Nombre = nombre,
                    Email = email,
                    Password = pEncript,
                    Estado = true,
                    fechaBaja = null,
                    Administrador = administrador
                };
                try
                {
                    db.Usuarios.Add(usuario);

                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    throw new Exception(ex.Message);
                }
                return usuario;
            }
        }
        public static List<Usuario> ListarUsuarios()
        {
            using (var cn = new Conexion())
            {
                List<Usuario> usuarios = cn.Usuarios.ToList();
                return usuarios;

            }
        }
        public static void EliminarUsuario(int id)
        {
            using (var db = new Conexion())
            {
                var usuario = db.Usuarios.FirstOrDefault(a => a.idUsuario == id);
                var eliminado = db.Usuarios.Remove(usuario);
                db.SaveChanges();
            }
        }
        public static string HashPassword(string password)
        {
            SHA256 hash = SHA256.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = hash.ComputeHash(encoding.GetBytes(password));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);

            return sb.ToString();

        }
        public static Usuario Login(string email, string password)
        {
            using (var db = new Conexion())
            {
                var pEncript = HashPassword(password);
                var usu2 = db.Usuarios.ToList();
                var usu = db.Usuarios.SingleOrDefault(uss => uss.Email == email && uss.Password == pEncript);
                return usu;
            }
        }
        public static Usuario RegistrarUsuario(int id, string nombre, string email, string password, string direccion)
        {
            using (var db = new Conexion())
            {
                Usuario usuario = new Usuario();

                usuario = db.Usuarios.FirstOrDefault(a => a.idUsuario == id);
                var pEncript = HashPassword(password);

                usuario = new Usuario()
                {
                    idUsuario = id,
                    Nombre = nombre,
                    Email = email,
                    Password = pEncript,
                    Estado = true,
                    fechaBaja = null,

                    Administrador = false
                };

                try
                {
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    throw new Exception(ex.Message);
                }
                return usuario;

            }
        }

    }
}
