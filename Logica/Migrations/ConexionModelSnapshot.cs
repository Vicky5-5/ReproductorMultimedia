﻿// <auto-generated />
using System;
using Logica.Contexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Logica.Migrations
{
    [DbContext(typeof(Conexion))]
    partial class ConexionModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Logica.Models.Canciones", b =>
                {
                    b.Property<int>("idCancion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idCancion"));

                    b.Property<string>("Album")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Artista")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("Duracion")
                        .HasColumnType("time");

                    b.Property<int>("Genero")
                        .HasColumnType("int");

                    b.Property<int>("NumeroLikes")
                        .HasColumnType("int");

                    b.Property<int>("NumeroReproducciones")
                        .HasColumnType("int");

                    b.Property<string>("RutaArchivo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RutaCaratulaAlbum")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("UsuarioDioLike")
                        .HasColumnType("bit");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("idCancion");

                    b.ToTable("Canciones");
                });

            modelBuilder.Entity("Logica.Models.CancionesFavoritas", b =>
                {
                    b.Property<int>("idFavorita")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idFavorita"));

                    b.Property<DateTime>("fecharAnadidaFavorita")
                        .HasColumnType("datetime2");

                    b.Property<int>("idCancion")
                        .HasColumnType("int");

                    b.Property<int>("idUsuario")
                        .HasColumnType("int");

                    b.HasKey("idFavorita");

                    b.HasIndex("idCancion");

                    b.HasIndex("idUsuario");

                    b.ToTable("Favoritas");
                });

            modelBuilder.Entity("Logica.Models.ListaReproduccion", b =>
                {
                    b.Property<Guid>("idLista")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CancionidCancion")
                        .HasColumnType("int");

                    b.Property<string>("NombreLista")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsuarioidUsuario")
                        .HasColumnType("int");

                    b.Property<DateTime>("cancionAnadida")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("fechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<int>("idCancion")
                        .HasColumnType("int");

                    b.Property<int>("idUsuario")
                        .HasColumnType("int");

                    b.HasKey("idLista");

                    b.HasIndex("CancionidCancion");

                    b.HasIndex("UsuarioidUsuario");

                    b.ToTable("ListaReproduccion");
                });

            modelBuilder.Entity("Logica.Models.Usuario", b =>
                {
                    b.Property<int>("idUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idUsuario"));

                    b.Property<bool>("Administrador")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Estado")
                        .HasColumnType("bit");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<DateTime?>("fechaBaja")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("fechaRegistro")
                        .HasColumnType("datetime2");

                    b.HasKey("idUsuario");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("Logica.Models.CancionesFavoritas", b =>
                {
                    b.HasOne("Logica.Models.Canciones", "Cancion")
                        .WithMany()
                        .HasForeignKey("idCancion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Logica.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("idUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cancion");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Logica.Models.ListaReproduccion", b =>
                {
                    b.HasOne("Logica.Models.Canciones", "Cancion")
                        .WithMany("ListaReproduccion")
                        .HasForeignKey("CancionidCancion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Logica.Models.Usuario", "Usuario")
                        .WithMany("ListasReproduccion")
                        .HasForeignKey("UsuarioidUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cancion");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Logica.Models.Canciones", b =>
                {
                    b.Navigation("ListaReproduccion");
                });

            modelBuilder.Entity("Logica.Models.Usuario", b =>
                {
                    b.Navigation("ListasReproduccion");
                });
#pragma warning restore 612, 618
        }
    }
}
