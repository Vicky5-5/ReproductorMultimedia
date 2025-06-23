using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Logica.Models;

namespace Logica.Contexto
{
    public class Conexion : DbContext
    {
        public Conexion()
        {
        }

        public Conexion(DbContextOptions<Conexion> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ListaReproduccion> ListaReproduccion { get; set; }
        public DbSet<Canciones> Canciones { get; set; }
    }

}
