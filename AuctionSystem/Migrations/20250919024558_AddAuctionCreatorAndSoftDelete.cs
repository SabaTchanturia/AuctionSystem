using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddAuctionCreatorAndSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Auctions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Auctions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_CreatedByUserId",
                table: "Auctions",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_Users_CreatedByUserId",
                table: "Auctions",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_Users_CreatedByUserId",
                table: "Auctions");

            migrationBuilder.DropIndex(
                name: "IX_Auctions_CreatedByUserId",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Auctions");
        }
    }
}
