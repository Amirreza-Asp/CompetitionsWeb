using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Competitions.Persistence.Migrations
{
    public partial class AddExtracurricularAndExtracurricularTimeTables : Migration
    {
        protected override void Up ( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.CreateTable(
                name: "Extracurricular" ,
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier" , nullable: false) ,
                    Name = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    SportId = table.Column<long>(type: "bigint" , nullable: false) ,
                    PlaceId = table.Column<Guid>(type: "uniqueidentifier" , nullable: false) ,
                    CoachId = table.Column<long>(type: "bigint" , nullable: false) ,
                    AudienceTypeId = table.Column<long>(type: "bigint" , nullable: false) ,
                    Capacity = table.Column<int>(type: "int" , nullable: false) ,
                    StartPutOn = table.Column<DateTime>(type: "datetime2" , nullable: false) ,
                    EndPutOn = table.Column<DateTime>(type: "datetime2" , nullable: false) ,
                    Gender = table.Column<bool>(type: "bit" , nullable: false) ,
                    StartRegister = table.Column<DateTime>(type: "datetime2" , nullable: false) ,
                    EndRegister = table.Column<DateTime>(type: "datetime2" , nullable: false) ,
                    Description = table.Column<string>(type: "nvarchar(max)" , nullable: false)
                } ,
                constraints: table =>
                {
                    table.PrimaryKey("PK_Extracurricular" , x => x.Id);
                    table.ForeignKey(
                        name: "FK_Extracurricular_AudienceType_AudienceTypeId" ,
                        column: x => x.AudienceTypeId ,
                        principalTable: "AudienceType" ,
                        principalColumn: "Id" ,
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Extracurricular_Coach_CoachId" ,
                        column: x => x.CoachId ,
                        principalTable: "Coach" ,
                        principalColumn: "Id" ,
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Extracurricular_Place_PlaceId" ,
                        column: x => x.PlaceId ,
                        principalTable: "Place" ,
                        principalColumn: "Id" ,
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Extracurricular_Sport_SportId" ,
                        column: x => x.SportId ,
                        principalTable: "Sport" ,
                        principalColumn: "Id" ,
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ExtracurricularTime" ,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint" , nullable: false)
                        .Annotation("SqlServer:Identity" , "1, 1") ,
                    Day = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    Hour = table.Column<int>(type: "int" , nullable: false) ,
                    Min = table.Column<int>(type: "int" , nullable: false) ,
                    ExtracurricularId = table.Column<Guid>(type: "uniqueidentifier" , nullable: false)
                } ,
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtracurricularTime" , x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtracurricularTime_Extracurricular_ExtracurricularId" ,
                        column: x => x.ExtracurricularId ,
                        principalTable: "Extracurricular" ,
                        principalColumn: "Id" ,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Extracurricular_AudienceTypeId" ,
                table: "Extracurricular" ,
                column: "AudienceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Extracurricular_CoachId" ,
                table: "Extracurricular" ,
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Extracurricular_PlaceId" ,
                table: "Extracurricular" ,
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Extracurricular_SportId" ,
                table: "Extracurricular" ,
                column: "SportId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtracurricularTime_ExtracurricularId" ,
                table: "ExtracurricularTime" ,
                column: "ExtracurricularId");
        }

        protected override void Down ( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropTable(
                name: "ExtracurricularTime");

            migrationBuilder.DropTable(
                name: "Extracurricular");
        }
    }
}
