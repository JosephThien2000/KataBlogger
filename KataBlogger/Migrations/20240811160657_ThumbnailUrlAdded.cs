using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KataBlogger.Migrations
{
    /// <inheritdoc />
    public partial class ThumbnailUrlAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "slug",
                table: "Pages",
                newName: "Slug");

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "Pages",
                newName: "slug");
        }
    }
}
