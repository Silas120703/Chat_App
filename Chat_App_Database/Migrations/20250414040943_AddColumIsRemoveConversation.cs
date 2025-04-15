using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat_App_Database.Migrations
{
    /// <inheritdoc />
    public partial class AddColumIsRemoveConversation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRemove",
                table: "Conversations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRemove",
                table: "Conversations");
        }
    }
}
