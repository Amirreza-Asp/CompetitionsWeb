using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Competitions.Persistence.Migrations
{
    public partial class AddMatchTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Match",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    TeamCount = table.Column<byte>(type: "tinyint", nullable: false),
                    RegistrationsNumber = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartRegsiter = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndRegister = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartPutOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndPutOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Image = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    FestivalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PlaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SportId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Match_Festival_FestivalId",
                        column: x => x.FestivalId,
                        principalTable: "Festival",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Match_Place_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Place",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Match_Sport_SportId",
                        column: x => x.SportId,
                        principalTable: "Sport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchAudienceType",
                columns: table => new
                {
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AudienceTypeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchAudienceType", x => new { x.MatchId, x.AudienceTypeId });
                    table.ForeignKey(
                        name: "FK_MatchAudienceType_AudienceType_AudienceTypeId",
                        column: x => x.AudienceTypeId,
                        principalTable: "AudienceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchAudienceType_Match_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Match",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchAward",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Score = table.Column<byte>(type: "tinyint", nullable: false),
                    Prize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchAward", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchAward_Match_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Match",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchConditions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Regulations = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Free = table.Column<bool>(type: "bit", nullable: false),
                    Payment = table.Column<int>(type: "int", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchConditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchConditions_Match_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Match",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchDocument",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EvidenceId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchDocument_Evidence_EvidenceId",
                        column: x => x.EvidenceId,
                        principalTable: "Evidence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchDocument_Match_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Match",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Match_FestivalId",
                table: "Match",
                column: "FestivalId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_PlaceId",
                table: "Match",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_SportId",
                table: "Match",
                column: "SportId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchAudienceType_AudienceTypeId",
                table: "MatchAudienceType",
                column: "AudienceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchAward_MatchId",
                table: "MatchAward",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchConditions_MatchId",
                table: "MatchConditions",
                column: "MatchId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MatchDocument_EvidenceId",
                table: "MatchDocument",
                column: "EvidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchDocument_MatchId",
                table: "MatchDocument",
                column: "MatchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchAudienceType");

            migrationBuilder.DropTable(
                name: "MatchAward");

            migrationBuilder.DropTable(
                name: "MatchConditions");

            migrationBuilder.DropTable(
                name: "MatchDocument");

            migrationBuilder.DropTable(
                name: "Match");
        }
    }
}
