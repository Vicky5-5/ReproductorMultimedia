using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logica.Migrations
{
    /// <inheritdoc />
    public partial class añadirFavoritos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favoritas_Canciones_CancionId",
                table: "Favoritas");

            migrationBuilder.DropForeignKey(
                name: "FK_Favoritas_Usuarios_UsuarioId",
                table: "Favoritas");

            migrationBuilder.DropIndex(
                name: "IX_Favoritas_CancionId",
                table: "Favoritas");

            migrationBuilder.DropIndex(
                name: "IX_Favoritas_UsuarioId",
                table: "Favoritas");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Favoritas",
                newName: "idUsuario");

            migrationBuilder.RenameColumn(
                name: "CancionId",
                table: "Favoritas",
                newName: "idCancion");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Favoritas",
                newName: "idFavorita");

            migrationBuilder.AddColumn<int>(
                name: "CancionidCancion",
                table: "Favoritas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioidUsuario",
                table: "Favoritas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Favoritas_CancionidCancion",
                table: "Favoritas",
                column: "CancionidCancion");

            migrationBuilder.CreateIndex(
                name: "IX_Favoritas_UsuarioidUsuario",
                table: "Favoritas",
                column: "UsuarioidUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Favoritas_Canciones_CancionidCancion",
                table: "Favoritas",
                column: "CancionidCancion",
                principalTable: "Canciones",
                principalColumn: "idCancion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favoritas_Usuarios_UsuarioidUsuario",
                table: "Favoritas",
                column: "UsuarioidUsuario",
                principalTable: "Usuarios",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favoritas_Canciones_CancionidCancion",
                table: "Favoritas");

            migrationBuilder.DropForeignKey(
                name: "FK_Favoritas_Usuarios_UsuarioidUsuario",
                table: "Favoritas");

            migrationBuilder.DropIndex(
                name: "IX_Favoritas_CancionidCancion",
                table: "Favoritas");

            migrationBuilder.DropIndex(
                name: "IX_Favoritas_UsuarioidUsuario",
                table: "Favoritas");

            migrationBuilder.DropColumn(
                name: "CancionidCancion",
                table: "Favoritas");

            migrationBuilder.DropColumn(
                name: "UsuarioidUsuario",
                table: "Favoritas");

            migrationBuilder.RenameColumn(
                name: "idUsuario",
                table: "Favoritas",
                newName: "UsuarioId");

            migrationBuilder.RenameColumn(
                name: "idCancion",
                table: "Favoritas",
                newName: "CancionId");

            migrationBuilder.RenameColumn(
                name: "idFavorita",
                table: "Favoritas",
                newName: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Favoritas_CancionId",
                table: "Favoritas",
                column: "CancionId");

            migrationBuilder.CreateIndex(
                name: "IX_Favoritas_UsuarioId",
                table: "Favoritas",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favoritas_Canciones_CancionId",
                table: "Favoritas",
                column: "CancionId",
                principalTable: "Canciones",
                principalColumn: "idCancion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favoritas_Usuarios_UsuarioId",
                table: "Favoritas",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
