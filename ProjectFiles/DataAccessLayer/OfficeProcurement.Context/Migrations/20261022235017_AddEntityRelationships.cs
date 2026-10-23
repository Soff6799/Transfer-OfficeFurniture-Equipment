using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeProcurement.Context.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ContractId",
                table: "TransferActs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "TransferActItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TransferActId",
                table: "TransferActItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                table: "Products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "Contracts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                table: "Contracts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TransferActs_ContractId",
                table: "TransferActs",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferActItems_ProductId",
                table: "TransferActItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferActItems_TransferActId",
                table: "TransferActItems",
                column: "TransferActId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierId",
                table: "Products",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ClientId",
                table: "Contracts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_SupplierId",
                table: "Contracts",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Clients_ClientId",
                table: "Contracts",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Suppliers_SupplierId",
                table: "Contracts",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Suppliers_SupplierId",
                table: "Products",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferActItems_Products_ProductId",
                table: "TransferActItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferActItems_TransferActs_TransferActId",
                table: "TransferActItems",
                column: "TransferActId",
                principalTable: "TransferActs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferActs_Contracts_ContractId",
                table: "TransferActs",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Clients_ClientId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Suppliers_SupplierId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Suppliers_SupplierId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferActItems_Products_ProductId",
                table: "TransferActItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferActItems_TransferActs_TransferActId",
                table: "TransferActItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferActs_Contracts_ContractId",
                table: "TransferActs");

            migrationBuilder.DropIndex(
                name: "IX_TransferActs_ContractId",
                table: "TransferActs");

            migrationBuilder.DropIndex(
                name: "IX_TransferActItems_ProductId",
                table: "TransferActItems");

            migrationBuilder.DropIndex(
                name: "IX_TransferActItems_TransferActId",
                table: "TransferActItems");

            migrationBuilder.DropIndex(
                name: "IX_Products_SupplierId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ClientId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_SupplierId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "TransferActs");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "TransferActItems");

            migrationBuilder.DropColumn(
                name: "TransferActId",
                table: "TransferActItems");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Contracts");
        }
    }
}
