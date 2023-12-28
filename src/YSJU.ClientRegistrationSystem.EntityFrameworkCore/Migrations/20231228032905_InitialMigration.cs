using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YSJU.ClientRegistrationSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientDetails_Products_ProductId",
                table: "ClientDetails");

            migrationBuilder.DropIndex(
                name: "IX_ClientDetails_ProductId",
                table: "ClientDetails");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ClientDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "ClientDetails",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ClientDetails_ProductId",
                table: "ClientDetails",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientDetails_Products_ProductId",
                table: "ClientDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
