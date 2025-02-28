using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPowerElectricAPI.Migrations
{
    /// <inheritdoc />
    public partial class Nominas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nomina",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    horasTrabajadas = table.Column<double>(type: "float", nullable: false),
                    SalarioEstandar = table.Column<double>(type: "float", nullable: true),
                    SalarioPlus = table.Column<double>(type: "float", nullable: true),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NoSemana = table.Column<int>(type: "int", nullable: false),
                    InicioSemana = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinSemana = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdTrabajador = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nomina", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nomina_Trabajador_IdTrabajador",
                        column: x => x.IdTrabajador,
                        principalTable: "Trabajador",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nomina_IdTrabajador",
                table: "Nomina",
                column: "IdTrabajador");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nomina");
        }
    }
}
