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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "Server=DESKTOP-F14E1IH\\SQLEXPRESS;Database=ReproductorMusica;Trusted_Connection=True;TrustServerCertificate=True;"; // o desde config
                optionsBuilder.UseSqlServer(connectionString);
            }
        }


    }

}
