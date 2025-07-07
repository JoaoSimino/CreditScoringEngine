using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreditScoringEngine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdcionandoDemaisEntidadesERelacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PropostaCreditoId",
                table: "Clientes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "PropostasCredito",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValorSolicitado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataProposta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScoreValor = table.Column<int>(type: "int", nullable: false),
                    ScoreFaixa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Justificativa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostasCredito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostasCredito_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropostasCredito_ClienteId",
                table: "PropostasCredito",
                column: "ClienteId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropostasCredito");

            migrationBuilder.DropColumn(
                name: "PropostaCreditoId",
                table: "Clientes");
        }
    }
}
