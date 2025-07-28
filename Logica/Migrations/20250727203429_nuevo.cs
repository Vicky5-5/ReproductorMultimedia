using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logica.Migrations
{
    /// <inheritdoc />
    public partial class nuevo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Canciones",
                columns: table => new
                {
                    idCancion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Artista = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Album = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Duracion = table.Column<TimeSpan>(type: "time", nullable: false),
                    Genero = table.Column<int>(type: "int", nullable: false),
                    NumeroReproducciones = table.Column<int>(type: "int", nullable: false),
                    NumeroLikes = table.Column<int>(type: "int", nullable: false),
                    RutaArchivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RutaCaratulaAlbum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioDioLike = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Canciones", x => x.idCancion);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    idUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    fechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fechaBaja = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Administrador = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.idUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Favoritas",
                columns: table => new
                {
                    idFavorita = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUsuario = table.Column<int>(type: "int", nullable: false),
                    fecharAnadidaFavorita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    idCancion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favoritas", x => x.idFavorita);
                    table.ForeignKey(
                        name: "FK_Favoritas_Canciones_idCancion",
                        column: x => x.idCancion,
                        principalTable: "Canciones",
                        principalColumn: "idCancion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favoritas_Usuarios_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListaReproduccion",
                columns: table => new
                {
                    idLista = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NombreLista = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cancionAnadida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    idCancion = table.Column<int>(type: "int", nullable: false),
                    idUsuario = table.Column<int>(type: "int", nullable: false),
                    UsuarioidUsuario = table.Column<int>(type: "int", nullable: false),
                    CancionidCancion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListaReproduccion", x => x.idLista);
                    table.ForeignKey(
                        name: "FK_ListaReproduccion_Canciones_CancionidCancion",
                        column: x => x.CancionidCancion,
                        principalTable: "Canciones",
                        principalColumn: "idCancion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListaReproduccion_Usuarios_UsuarioidUsuario",
                        column: x => x.UsuarioidUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favoritas_idCancion",
                table: "Favoritas",
                column: "idCancion");

            migrationBuilder.CreateIndex(
                name: "IX_Favoritas_idUsuario",
                table: "Favoritas",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_ListaReproduccion_CancionidCancion",
                table: "ListaReproduccion",
                column: "CancionidCancion");

            migrationBuilder.CreateIndex(
                name: "IX_ListaReproduccion_UsuarioidUsuario",
                table: "ListaReproduccion",
                column: "UsuarioidUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favoritas");

            migrationBuilder.DropTable(
                name: "ListaReproduccion");

            migrationBuilder.DropTable(
                name: "Canciones");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
