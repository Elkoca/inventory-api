using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventory_api.Migrations
{
    public partial class Currencyfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Currency_Products_CurrencyId",
                table: "Currency");

            migrationBuilder.CreateIndex(
                name: "IX_Currency_ProductId",
                table: "Currency",
                column: "ProductId",
                unique: true,
                filter: "[ProductId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Currency_Products_ProductId",
                table: "Currency",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Currency_Products_ProductId",
                table: "Currency");

            migrationBuilder.DropIndex(
                name: "IX_Currency_ProductId",
                table: "Currency");

            migrationBuilder.AddForeignKey(
                name: "FK_Currency_Products_CurrencyId",
                table: "Currency",
                column: "CurrencyId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
