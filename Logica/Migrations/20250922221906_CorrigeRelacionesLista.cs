using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logica.Migrations
{
    /// <inheritdoc />
    public partial class CorrigeRelacionesLista : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListaReproduccion_Canciones_CancionidCancion",
                table: "ListaReproduccion");

            migrationBuilder.DropForeignKey(
                name: "FK_ListaReproduccion_Usuarios_UsuarioidUsuario",
                table: "ListaReproduccion");

            migrationBuilder.DropIndex(
                name: "IX_ListaReproduccion_CancionidCancion",
                table: "ListaReproduccion");

            migrationBuilder.DropColumn(
                name: "CancionidCancion",
                table: "ListaReproduccion");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioidUsuario",
                table: "ListaReproduccion",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CancionesidCancion",
                table: "ListaReproduccion",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ListaReproduccion_CancionesidCancion",
                table: "ListaReproduccion",
                column: "CancionesidCancion");

            migrationBuilder.CreateIndex(
                name: "IX_ListaReproduccion_idCancion",
                table: "ListaReproduccion",
                column: "idCancion");

            migrationBuilder.CreateIndex(
                name: "IX_ListaReproduccion_idUsuario",
                table: "ListaReproduccion",
                column: "idUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_ListaReproduccion_Canciones_CancionesidCancion",
                table: "ListaReproduccion",
                column: "CancionesidCancion",
                principalTable: "Canciones",
                principalColumn: "idCancion");

            migrationBuilder.AddForeignKey(
                name: "FK_ListaReproduccion_Canciones_idCancion",
                table: "ListaReproduccion",
                column: "idCancion",
                principalTable: "Canciones",
                principalColumn: "idCancion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ListaReproduccion_Usuarios_UsuarioidUsuario",
                table: "ListaReproduccion",
                column: "UsuarioidUsuario",
                principalTable: "Usuarios",
                principalColumn: "idUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_ListaReproduccion_Usuarios_idUsuario",
                table: "ListaReproduccion",
                column: "idUsuario",
                principalTable: "Usuarios",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListaReproduccion_Canciones_CancionesidCancion",
                table: "ListaReproduccion");

            migrationBuilder.DropForeignKey(
                name: "FK_ListaReproduccion_Canciones_idCancion",
                table: "ListaReproduccion");

            migrationBuilder.DropForeignKey(
                name: "FK_ListaReproduccion_Usuarios_UsuarioidUsuario",
                table: "ListaReproduccion");

            migrationBuilder.DropForeignKey(
                name: "FK_ListaReproduccion_Usuarios_idUsuario",
                table: "ListaReproduccion");

            migrationBuilder.DropIndex(
                name: "IX_ListaReproduccion_CancionesidCancion",
                table: "ListaReproduccion");

            migrationBuilder.DropIndex(
                name: "IX_ListaReproduccion_idCancion",
                table: "ListaReproduccion");

            migrationBuilder.DropIndex(
                name: "IX_ListaReproduccion_idUsuario",
                table: "ListaReproduccion");

            migrationBuilder.DropColumn(
                name: "CancionesidCancion",
                table: "ListaReproduccion");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioidUsuario",
                table: "ListaReproduccion",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CancionidCancion",
                table: "ListaReproduccion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ListaReproduccion_CancionidCancion",
                table: "ListaReproduccion",
                column: "CancionidCancion");

            migrationBuilder.AddForeignKey(
                name: "FK_ListaReproduccion_Canciones_CancionidCancion",
                table: "ListaReproduccion",
                column: "CancionidCancion",
                principalTable: "Canciones",
                principalColumn: "idCancion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ListaReproduccion_Usuarios_UsuarioidUsuario",
                table: "ListaReproduccion",
                column: "UsuarioidUsuario",
                principalTable: "Usuarios",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
