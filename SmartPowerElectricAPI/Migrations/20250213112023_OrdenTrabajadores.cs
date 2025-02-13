using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPowerElectricAPI.Migrations
{
    /// <inheritdoc />
    public partial class OrdenTrabajadores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProyectoTrabajador");

            migrationBuilder.CreateTable(
                name: "OrdenTrabajador",
                columns: table => new
                {
                    OrdensId = table.Column<int>(type: "int", nullable: false),
                    TrabajadoresId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenTrabajador", x => new { x.OrdensId, x.TrabajadoresId });
                    table.ForeignKey(
                        name: "FK_OrdenTrabajador_Orden_OrdensId",
                        column: x => x.OrdensId,
                        principalTable: "Orden",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdenTrabajador_Trabajador_TrabajadoresId",
                        column: x => x.TrabajadoresId,
                        principalTable: "Trabajador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdenTrabajador_TrabajadoresId",
                table: "OrdenTrabajador",
                column: "TrabajadoresId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdenTrabajador");

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
                name: "IX_ProyectoTrabajador_TrabajadoresId",
                table: "ProyectoTrabajador",
                column: "TrabajadoresId");
        }
    }
}
