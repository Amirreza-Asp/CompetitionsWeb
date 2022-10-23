using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Competitions.Persistence.Migrations
{
    public partial class ChangePutOnTypeFromDateTimeToDateTimeRangeInMatchTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PutOn",
                table: "Match",
                newName: "StartPutOn");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndPutOn",
                table: "Match",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndPutOn",
                table: "Match");

            migrationBuilder.RenameColumn(
                name: "StartPutOn",
                table: "Match",
                newName: "PutOn");
        }
    }
}
