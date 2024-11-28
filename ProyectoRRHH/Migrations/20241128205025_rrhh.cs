using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoRRHH.Migrations
{
    /// <inheritdoc />
    public partial class rrhh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Dni = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Roles = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Dni);
                });

            migrationBuilder.CreateTable(
                name: "ReciboSueldos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FechaCobro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    sueldo = table.Column<double>(type: "float", nullable: false),
                    UsuarioDni = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReciboSueldos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReciboSueldos_Usuarios_UsuarioDni",
                        column: x => x.UsuarioDni,
                        principalTable: "Usuarios",
                        principalColumn: "Dni");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReciboSueldos_UsuarioDni",
                table: "ReciboSueldos",
                column: "UsuarioDni");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReciboSueldos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
