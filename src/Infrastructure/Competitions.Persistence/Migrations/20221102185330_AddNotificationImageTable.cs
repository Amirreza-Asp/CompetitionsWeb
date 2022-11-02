using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Competitions.Persistence.Migrations
{
    public partial class AddNotificationImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InviteResult");

            migrationBuilder.DropTable(
                name: "InviteToMatch");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Notification");

            migrationBuilder.CreateTable(
                name: "NotificationImage",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Image = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    NotificationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationImage_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationImage_NotificationId",
                table: "NotificationImage",
                column: "NotificationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationImage");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Notification",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "InviteToMatch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvitedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InviterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    SendTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InviteToMatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InviteToMatch_Match_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Match",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InviteToMatch_User_InvitedId",
                        column: x => x.InvitedId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InviteToMatch_User_InviterId",
                        column: x => x.InviterId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InviteResult",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InviteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Accepted = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InviteResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InviteResult_InviteToMatch_InviteId",
                        column: x => x.InviteId,
                        principalTable: "InviteToMatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InviteResult_InviteId",
                table: "InviteResult",
                column: "InviteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InviteToMatch_InvitedId",
                table: "InviteToMatch",
                column: "InvitedId");

            migrationBuilder.CreateIndex(
                name: "IX_InviteToMatch_InviterId",
                table: "InviteToMatch",
                column: "InviterId");

            migrationBuilder.CreateIndex(
                name: "IX_InviteToMatch_MatchId",
                table: "InviteToMatch",
                column: "MatchId");
        }
    }
}
