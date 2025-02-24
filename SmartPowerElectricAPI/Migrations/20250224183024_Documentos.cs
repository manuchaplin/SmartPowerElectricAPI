using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPowerElectricAPI.Migrations
{
    /// <inheritdoc />
    public partial class Documentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentoCaducar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdTrabajador = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoCaducar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentoCaducar_Trabajador_IdTrabajador",
                        column: x => x.IdTrabajador,
                        principalTable: "Trabajador",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoCaducar_IdTrabajador",
                table: "DocumentoCaducar",
                column: "IdTrabajador");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentoCaducar");
        }
    }
}
