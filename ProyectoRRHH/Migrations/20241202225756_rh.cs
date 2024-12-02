using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoRRHH.Migrations
{
    /// <inheritdoc />
    public partial class rh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Licencias_Usuarios_UsuarioDni",
                table: "Licencias");

            migrationBuilder.AddForeignKey(
                name: "FK_Licencias_Usuarios_UsuarioDni",
                table: "Licencias",
                column: "UsuarioDni",
                principalTable: "Usuarios",
                principalColumn: "Dni",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Licencias_Usuarios_UsuarioDni",
                table: "Licencias");

            migrationBuilder.AddForeignKey(
                name: "FK_Licencias_Usuarios_UsuarioDni",
                table: "Licencias",
                column: "UsuarioDni",
                principalTable: "Usuarios",
                principalColumn: "Dni");
        }
    }
}
