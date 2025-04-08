using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class AgregarDepositos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Depositos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Area = table.Column<int>(type: "int", nullable: false),
                    Tamanio = table.Column<int>(type: "int", nullable: false),
                    Climatizacion = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depositos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepositoPromocion",
                columns: table => new
                {
                    ListaPromocionesId = table.Column<int>(type: "int", nullable: false),
                    listaDepositosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositoPromocion", x => new { x.ListaPromocionesId, x.listaDepositosId });
                    table.ForeignKey(
                        name: "FK_DepositoPromocion_Depositos_listaDepositosId",
                        column: x => x.listaDepositosId,
                        principalTable: "Depositos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepositoPromocion_Promociones_ListaPromocionesId",
                        column: x => x.ListaPromocionesId,
                        principalTable: "Promociones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepositoRangoDeFechas",
                columns: table => new
                {
                    ListaDisponibilidadId = table.Column<int>(type: "int", nullable: false),
                    listaDepositosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositoRangoDeFechas", x => new { x.ListaDisponibilidadId, x.listaDepositosId });
                    table.ForeignKey(
                        name: "FK_DepositoRangoDeFechas_Depositos_listaDepositosId",
                        column: x => x.listaDepositosId,
                        principalTable: "Depositos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepositoRangoDeFechas_RangoDeFechas_ListaDisponibilidadId",
                        column: x => x.ListaDisponibilidadId,
                        principalTable: "RangoDeFechas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepositoPromocion_listaDepositosId",
                table: "DepositoPromocion",
                column: "listaDepositosId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositoRangoDeFechas_listaDepositosId",
                table: "DepositoRangoDeFechas",
                column: "listaDepositosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepositoPromocion");

            migrationBuilder.DropTable(
                name: "DepositoRangoDeFechas");

            migrationBuilder.DropTable(
                name: "Depositos");
        }
    }
}
