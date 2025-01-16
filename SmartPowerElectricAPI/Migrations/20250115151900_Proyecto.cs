using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPowerElectricAPI.Migrations
{
    /// <inheritdoc />
    public partial class Proyecto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdProyecto",
                table: "Material",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Proyecto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdCliente = table.Column<int>(type: "int", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaEliminado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Eliminado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyecto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proyecto_Cliente_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Cliente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProyectoTrabajador",
                columns: table => new
                {
                    ProyectosId = table.Column<int>(type: "int", nullable: false),
                    TrabajadoresId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProyectoTrabajador", x => new { x.ProyectosId, x.TrabajadoresId });
                    table.ForeignKey(
                        name: "FK_ProyectoTrabajador_Proyecto_ProyectosId",
                        column: x => x.ProyectosId,
                        principalTable: "Proyecto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProyectoTrabajador_Trabajador_TrabajadoresId",
                        column: x => x.TrabajadoresId,
                        principalTable: "Trabajador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Material_IdProyecto",
                table: "Material",
                column: "IdProyecto");

            migrationBuilder.CreateIndex(
                name: "IX_Proyecto_IdCliente",
                table: "Proyecto",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_ProyectoTrabajador_TrabajadoresId",
                table: "ProyectoTrabajador",
                column: "TrabajadoresId");

            migrationBuilder.AddForeignKey(
                name: "FK_Material_Proyecto_IdProyecto",
                table: "Material",
                column: "IdProyecto",
                principalTable: "Proyecto",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Material_Proyecto_IdProyecto",
                table: "Material");

            migrationBuilder.DropTable(
                name: "ProyectoTrabajador");

            migrationBuilder.DropTable(
                name: "Proyecto");

            migrationBuilder.DropIndex(
                name: "IX_Material_IdProyecto",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "IdProyecto",
                table: "Material");
        }
    }
}
