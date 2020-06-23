using Microsoft.EntityFrameworkCore.Migrations;

namespace LPFW.ORM.Migrations
{
    public partial class lpApp002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lyricName",
                table: "MusicCores");

            migrationBuilder.AddColumn<string>(
                name: "lyricPath",
                table: "MusicCores",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lyricPath",
                table: "MusicCores");

            migrationBuilder.AddColumn<string>(
                name: "lyricName",
                table: "MusicCores",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
