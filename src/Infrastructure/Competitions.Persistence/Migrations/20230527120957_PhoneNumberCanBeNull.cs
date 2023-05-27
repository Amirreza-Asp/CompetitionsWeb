using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Competitions.Persistence.Migrations
{
    public partial class PhoneNumberCanBeNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "User",
                type: "varchar(11)",
                unicode: false,
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(11)",
                oldUnicode: false,
                oldMaxLength: 11);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "User",
                type: "varchar(11)",
                unicode: false,
                maxLength: 11,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(11)",
                oldUnicode: false,
                oldMaxLength: 11,
                oldNullable: true);
        }
    }
}
