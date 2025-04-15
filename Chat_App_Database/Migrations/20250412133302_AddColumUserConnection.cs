using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat_App_Database.Migrations
{
    /// <inheritdoc />
    public partial class AddColumUserConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BrowserId",
                table: "UserConnection",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrowserId",
                table: "UserConnection");
        }
    }
}
