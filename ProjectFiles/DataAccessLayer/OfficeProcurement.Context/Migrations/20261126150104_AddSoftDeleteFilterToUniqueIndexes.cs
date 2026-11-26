using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeProcurement.Context.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteFilterToUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransferActs_Number",
                table: "TransferActs");

            migrationBuilder.DropIndex(
                name: "IX_TransferActItems_StockNumber",
                table: "TransferActItems");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_Inn",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Products_StockNumber",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_Number",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Clients_Inn",
                table: "Clients");

            migrationBuilder.CreateIndex(
                name: "IX_TransferActs_Number",
                table: "TransferActs",
                column: "Number",
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransferActItems_StockNumber",
                table: "TransferActItems",
                column: "StockNumber",
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Inn",
                table: "Suppliers",
                column: "Inn",
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StockNumber",
                table: "Products",
                column: "StockNumber",
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_Number",
                table: "Contracts",
                column: "Number",
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Inn",
                table: "Clients",
                column: "Inn",
                unique: true,
                filter: "\"DeletedAt\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransferActs_Number",
                table: "TransferActs");

            migrationBuilder.DropIndex(
                name: "IX_TransferActItems_StockNumber",
                table: "TransferActItems");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_Inn",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Products_StockNumber",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_Number",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Clients_Inn",
                table: "Clients");

            migrationBuilder.CreateIndex(
                name: "IX_TransferActs_Number",
                table: "TransferActs",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransferActItems_StockNumber",
                table: "TransferActItems",
                column: "StockNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Inn",
                table: "Suppliers",
                column: "Inn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_StockNumber",
                table: "Products",
                column: "StockNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_Number",
                table: "Contracts",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Inn",
                table: "Clients",
                column: "Inn",
                unique: true);
        }
    }
}
