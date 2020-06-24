using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LPFW.ORM.Migrations
{
    public partial class lpApp003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Power1Id",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserPwd",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    SingerName = table.Column<string>(nullable: true),
                    IssueTime = table.Column<DateTime>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    PhotoUrl = table.Column<string>(nullable: true),
                    MusicEntityId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Albums_Albums_MusicEntityId",
                        column: x => x.MusicEntityId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Powers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Admin = table.Column<string>(nullable: true),
                    User = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Powers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AlbumWithMusics",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AlbumId = table.Column<Guid>(nullable: true),
                    MusicId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumWithMusics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlbumWithMusics_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlbumWithMusics_MusicType_MusicId",
                        column: x => x.MusicId,
                        principalTable: "MusicType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MusicManagements",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    GroundingTime = table.Column<string>(nullable: true),
                    AlbumId = table.Column<Guid>(nullable: true),
                    MusicId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicManagements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MusicManagements_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MusicManagements_Music_MusicId",
                        column: x => x.MusicId,
                        principalTable: "Music",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Power1Id",
                table: "AspNetUsers",
                column: "Power1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_MusicEntityId",
                table: "Albums",
                column: "MusicEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumWithMusics_AlbumId",
                table: "AlbumWithMusics",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumWithMusics_MusicId",
                table: "AlbumWithMusics",
                column: "MusicId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicManagements_AlbumId",
                table: "MusicManagements",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicManagements_MusicId",
                table: "MusicManagements",
                column: "MusicId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Powers_Power1Id",
                table: "AspNetUsers",
                column: "Power1Id",
                principalTable: "Powers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Powers_Power1Id",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AlbumWithMusics");

            migrationBuilder.DropTable(
                name: "MusicManagements");

            migrationBuilder.DropTable(
                name: "Powers");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Power1Id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Login",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Power1Id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserPwd",
                table: "AspNetUsers");
        }
    }
}
