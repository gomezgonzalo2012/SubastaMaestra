using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubastaMaestra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddinNewStatesToProductandAuction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "State",
                table: "Products",
                newName: "CurrentState");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Auctions",
                newName: "CurrentState");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentState",
                table: "Products",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "CurrentState",
                table: "Auctions",
                newName: "State");
        }
    }
}
