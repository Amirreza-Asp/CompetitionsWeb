using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Competitions.Persistence.Migrations
{
    public partial class AddExtracurricularUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Team",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 10, 16, 16, 37, 24, 362, DateTimeKind.Local).AddTicks(8258));

            migrationBuilder.CreateTable(
                name: "ExtracurricularUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsPay = table.Column<bool>(type: "bit", nullable: false),
                    JoinTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExtracurricularId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtracurricularUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtracurricularUser_Extracurricular_ExtracurricularId",
                        column: x => x.ExtracurricularId,
                        principalTable: "Extracurricular",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExtracurricularUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExtracurricularUser_ExtracurricularId",
                table: "ExtracurricularUser",
                column: "ExtracurricularId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtracurricularUser_UserId",
                table: "ExtracurricularUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExtracurricularUser");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Team",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 16, 16, 37, 24, 362, DateTimeKind.Local).AddTicks(8258),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
