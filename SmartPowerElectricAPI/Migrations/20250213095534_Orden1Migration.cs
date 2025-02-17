using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPowerElectricAPI.Migrations
{
    /// <inheritdoc />
    public partial class Orden1Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Material_Proyecto_IdProyecto",
                table: "Material");

            migrationBuilder.RenameColumn(
                name: "IdProyecto",
                table: "Material",
                newName: "IdOrden");

            migrationBuilder.RenameIndex(
                name: "IX_Material_IdProyecto",
                table: "Material",
                newName: "IX_Material_IdOrden");

            migrationBuilder.CreateTable(
                name: "Orden",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroOrden = table.Column<int>(type: "int", nullable: false),
                    OrdenFinalizada = table.Column<bool>(type: "bit", nullable: true),
                    CosteManoObra = table.Column<double>(type: "float", nullable: true),
                    Cobrado = table.Column<double>(type: "float", nullable: true),
                    HorasEstimadas = table.Column<double>(type: "float", nullable: true),
                    IdProyecto = table.Column<int>(type: "int", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaEliminado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Eliminado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orden", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orden_Proyecto_IdProyecto",
                        column: x => x.IdProyecto,
                        principalTable: "Proyecto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orden_IdProyecto",
                table: "Orden",
                column: "IdProyecto");

            migrationBuilder.AddForeignKey(
                name: "FK_Material_Orden_IdOrden",
                table: "Material",
                column: "IdOrden",
                principalTable: "Orden",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Material_Orden_IdOrden",
                table: "Material");

            migrationBuilder.DropTable(
                name: "Orden");

            migrationBuilder.RenameColumn(
                name: "IdOrden",
                table: "Material",
                newName: "IdProyecto");

            migrationBuilder.RenameIndex(
                name: "IX_Material_IdOrden",
                table: "Material",
                newName: "IX_Material_IdProyecto");

            migrationBuilder.AddForeignKey(
                name: "FK_Material_Proyecto_IdProyecto",
                table: "Material",
                column: "IdProyecto",
                principalTable: "Proyecto",
                principalColumn: "Id");
        }
    }
}
