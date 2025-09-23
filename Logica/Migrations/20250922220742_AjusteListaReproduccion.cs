using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logica.Migrations
{
    /// <inheritdoc />
    public partial class AjusteListaReproduccion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ListaReproduccion",
                table: "ListaReproduccion");

            migrationBuilder.AddColumn<int>(
                name: "idListaNoGuid",
                table: "ListaReproduccion",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListaReproduccion",
                table: "ListaReproduccion",
                column: "idListaNoGuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ListaReproduccion",
                table: "ListaReproduccion");

            migrationBuilder.DropColumn(
                name: "idListaNoGuid",
                table: "ListaReproduccion");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListaReproduccion",
                table: "ListaReproduccion",
                column: "idLista");
        }
    }
}
