using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Competitions.Persistence.Migrations
{
    public partial class AddLeaderColumnToUserTeamTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Leader",
                table: "UserTeam",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Leader",
                table: "UserTeam");
        }
    }
}
