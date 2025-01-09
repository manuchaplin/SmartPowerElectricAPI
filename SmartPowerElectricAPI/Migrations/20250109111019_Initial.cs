using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPowerElectricAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefono = table.Column<int>(type: "int", nullable: true),
                    fechaCreacion = table.Column<DateTime>(type: "datetime", nullable: true),
                    fechaEliminado = table.Column<DateTime>(type: "datetime", nullable: true),
                    eliminado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TipoMaterial",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fechaCreacion = table.Column<DateTime>(type: "datetime", nullable: true),
                    fechaEliminado = table.Column<DateTime>(type: "datetime", nullable: true),
                    eliminado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoMaterial", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Trabajador",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    especialidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    telefono = table.Column<int>(type: "int", nullable: true),
                    direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    seguridadSocial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fechaInicioContrato = table.Column<DateTime>(type: "datetime", nullable: true),
                    fechaFinContrato = table.Column<DateTime>(type: "datetime", nullable: true),
                    cobroxHora = table.Column<double>(type: "float", nullable: true),
                    fechaCreacion = table.Column<DateTime>(type: "datetime", nullable: true),
                    fechaEliminado = table.Column<DateTime>(type: "datetime", nullable: true),
                    eliminado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trabajador", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UnidadMedida",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    uMedida = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadMedida", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    usuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefono = table.Column<int>(type: "int", nullable: true),
                    fechaCreacion = table.Column<DateTime>(type: "datetime", nullable: true),
                    fechaEliminado = table.Column<DateTime>(type: "datetime", nullable: true),
                    eliminado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    precio = table.Column<double>(type: "float", nullable: true),
                    cantidad = table.Column<double>(type: "float", nullable: true),
                    idTipoMaterial = table.Column<int>(type: "int", nullable: false),
                    idUnidadMedida = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaEliminado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Eliminado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.id);
                    table.ForeignKey(
                        name: "FK_Material_TipoMaterial",
                        column: x => x.idTipoMaterial,
                        principalTable: "TipoMaterial",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Material_UnidadMedida",
                        column: x => x.idUnidadMedida,
                        principalTable: "UnidadMedida",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Material_idTipoMaterial",
                table: "Material",
                column: "idTipoMaterial");

            migrationBuilder.CreateIndex(
                name: "IX_Material_idUnidadMedida",
                table: "Material",
                column: "idUnidadMedida");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "Trabajador");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "TipoMaterial");

            migrationBuilder.DropTable(
                name: "UnidadMedida");
        }
    }
}
