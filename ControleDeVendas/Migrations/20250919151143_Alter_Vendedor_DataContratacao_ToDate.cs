using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleDeVendas.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Vendedor_DataContratacao_ToDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Produto",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Produto");
        }
    }
}
