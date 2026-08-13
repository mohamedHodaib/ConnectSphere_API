using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConnectSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeNoActionwhendeletingbookmarkedusers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookmark_Users_BookmarkedUserId",
                table: "Bookmark");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookmark_Users_BookmarkedUserId",
                table: "Bookmark",
                column: "BookmarkedUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookmark_Users_BookmarkedUserId",
                table: "Bookmark");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookmark_Users_BookmarkedUserId",
                table: "Bookmark",
                column: "BookmarkedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
