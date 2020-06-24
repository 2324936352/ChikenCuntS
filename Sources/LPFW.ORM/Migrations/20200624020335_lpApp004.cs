using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LPFW.ORM.Migrations
{
    public partial class lpApp004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MusicTypeId",
                table: "Albums",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Albums_MusicTypeId",
                table: "Albums",
                column: "MusicTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_MusicType_MusicTypeId",
                table: "Albums",
                column: "MusicTypeId",
                principalTable: "MusicType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_MusicType_MusicTypeId",
                table: "Albums");

            migrationBuilder.DropIndex(
                name: "IX_Albums_MusicTypeId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "MusicTypeId",
                table: "Albums");
        }
    }
}
