using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionSystem.Migrations
{
    /// <inheritdoc />
    public partial class CarOnlyFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Auctions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Make",
                table: "Auctions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MileageKm",
                table: "Auctions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Auctions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Vin",
                table: "Auctions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Auctions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "Make",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "MileageKm",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "Vin",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Auctions");
        }
    }
}
