using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryBackend.Migrations
{
    /// <inheritdoc />
    public partial class Test2AddEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Orders_OrderEntityId",
                table: "Dishes");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_OrderEntityId",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "OrderEntityId",
                table: "Dishes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrderEntityId",
                table: "Dishes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_OrderEntityId",
                table: "Dishes",
                column: "OrderEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Orders_OrderEntityId",
                table: "Dishes",
                column: "OrderEntityId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
