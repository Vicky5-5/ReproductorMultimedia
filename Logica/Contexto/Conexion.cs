using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Logica.Models;
using Logica.Modelos_Auxiliares;

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
        public DbSet<CancionesFavoritas> Favoritas { get; set; }
        // Para la tabla de relación entre Usuario y Dispositivo. Asi aseguramos que 
        public DbSet<DispositivoUsuario> DispositivoUsuario { get; set; }
        public DbSet<VerificacionLogin> LoginVerificacion { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "Server=DESKTOP-F14E1IH\\SQLEXPRESS;Database=ReproductorMusica;Trusted_Connection=True;TrustServerCertificate=True;"; // o desde config
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ListaReproduccion>()
                .HasOne(l => l.Cancion)
                .WithMany()
                .HasForeignKey(l => l.idCancion)
                .HasConstraintName("FK_ListaReproduccion_Canciones_idCancion");

            modelBuilder.Entity<ListaReproduccion>()
                .HasOne(l => l.Usuario)
                .WithMany()
                .HasForeignKey(l => l.idUsuario)
                .HasConstraintName("FK_ListaReproduccion_Usuarios_idUsuario");
        }




    }

}
