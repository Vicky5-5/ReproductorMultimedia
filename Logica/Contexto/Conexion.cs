using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logica.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
