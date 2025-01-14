using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPowerElectricAPI.Migrations
{
    /// <inheritdoc />
    public partial class Update14012025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Material_TipoMaterial_IdTipoMaterial",
                table: "Material");

            migrationBuilder.DropForeignKey(
                name: "FK_Material_UnidadMedida_IdUnidadMedida",
                table: "Material");

            migrationBuilder.AlterColumn<int>(
                name: "IdUnidadMedida",
                table: "Material",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdTipoMaterial",
                table: "Material",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Material_TipoMaterial_IdTipoMaterial",
                table: "Material",
                column: "IdTipoMaterial",
                principalTable: "TipoMaterial",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Material_UnidadMedida_IdUnidadMedida",
                table: "Material",
                column: "IdUnidadMedida",
                principalTable: "UnidadMedida",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Material_TipoMaterial_IdTipoMaterial",
                table: "Material");

            migrationBuilder.DropForeignKey(
                name: "FK_Material_UnidadMedida_IdUnidadMedida",
                table: "Material");

            migrationBuilder.AlterColumn<int>(
                name: "IdUnidadMedida",
                table: "Material",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdTipoMaterial",
                table: "Material",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Material_TipoMaterial_IdTipoMaterial",
                table: "Material",
                column: "IdTipoMaterial",
                principalTable: "TipoMaterial",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Material_UnidadMedida_IdUnidadMedida",
                table: "Material",
                column: "IdUnidadMedida",
                principalTable: "UnidadMedida",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
