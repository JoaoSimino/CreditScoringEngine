using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreditScoringEngine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovendoPropostaCreditoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PropostaCreditoId",
                table: "Clientes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PropostaCreditoId",
                table: "Clientes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
