using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat_App_Database.Migrations
{
    /// <inheritdoc />
    public partial class AddColumConversation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountNotifications",
                table: "Conversations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountNotifications",
                table: "Conversations");
        }
    }
}
