using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPowerElectricAPI.Migrations
{
    /// <inheritdoc />
    public partial class UnidadMedidaUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "UnidadMedida",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "UnidadMedida",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEliminado",
                table: "UnidadMedida",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "UnidadMedida");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "UnidadMedida");

            migrationBuilder.DropColumn(
                name: "FechaEliminado",
                table: "UnidadMedida");
        }
    }
}
