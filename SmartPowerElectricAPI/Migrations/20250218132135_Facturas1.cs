using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPowerElectricAPI.Migrations
{
    /// <inheritdoc />
    public partial class Facturas1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Enrutamiento",
                table: "Trabajador",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroCuenta",
                table: "Trabajador",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Factura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MontoACobrar = table.Column<double>(type: "float", nullable: false),
                    EmailEnviado = table.Column<bool>(type: "bit", nullable: true),
                    FacturaCompletada = table.Column<bool>(type: "bit", nullable: true),
                    IdOrden = table.Column<int>(type: "int", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaEliminado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Eliminado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factura_Orden_IdOrden",
                        column: x => x.IdOrden,
                        principalTable: "Orden",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Factura_IdOrden",
                table: "Factura",
                column: "IdOrden");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Factura");

            migrationBuilder.DropColumn(
                name: "Enrutamiento",
                table: "Trabajador");

            migrationBuilder.DropColumn(
                name: "NumeroCuenta",
                table: "Trabajador");
        }
    }
}
