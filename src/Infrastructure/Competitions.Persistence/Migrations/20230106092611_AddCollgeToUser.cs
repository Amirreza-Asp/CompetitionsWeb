using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Competitions.Persistence.Migrations
{
    public partial class AddCollgeToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "College",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "College",
                table: "User");
        }
    }
}
