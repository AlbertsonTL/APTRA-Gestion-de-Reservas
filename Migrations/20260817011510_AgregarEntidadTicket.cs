using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APTRA_Gestion_de_Reservas.Migrations
{
    /// <inheritdoc />
    public partial class AgregarEntidadTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoValidacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Pasajero = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Documento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RutaId = table.Column<int>(type: "int", nullable: false),
                    Trayecto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaViaje = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Rutas_RutaId",
                        column: x => x.RutaId,
                        principalTable: "Rutas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CodigoValidacion",
                table: "Tickets",
                column: "CodigoValidacion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_RutaId",
                table: "Tickets",
                column: "RutaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");
        }
    }
}
