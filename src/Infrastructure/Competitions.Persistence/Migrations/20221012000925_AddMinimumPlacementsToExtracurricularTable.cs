using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Competitions.Persistence.Migrations
{
    public partial class AddMinimumPlacementsToExtracurricularTable : Migration
    {
        protected override void Up ( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.AddColumn<int>(
                name: "MinimumPlacements" ,
                table: "Extracurricular" ,
                type: "int" ,
                nullable: false ,
                defaultValue: 1);
        }

        protected override void Down ( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropColumn(
                name: "MinimumPlacements" ,
                table: "Extracurricular");
        }
    }
}
