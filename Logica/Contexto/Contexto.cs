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
    public class Contexto : DbContext
    {
        public DbSet<Usuario> Usuarios {get; set;}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-F14E1IH\\SQLEXPRESS;Database=ReproductorMusica;Trusted_Connection=True;");
        }
    }
}
