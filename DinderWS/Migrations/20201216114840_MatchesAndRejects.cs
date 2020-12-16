using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DinderWS.Migrations
{
    public partial class MatchesAndRejects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MatchId",
                table: "Experiences",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupSize = table.Column<byte>(nullable: false),
                    Gender = table.Column<byte>(nullable: false),
                    CuisineType = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(nullable: false),
                    AvgLongitude = table.Column<double>(nullable: false),
                    AvgLatitude = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rejects",
                columns: table => new
                {
                    ExperienceId = table.Column<string>(maxLength: 450, nullable: false),
                    MatchId = table.Column<long>(nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rejects", x => new { x.ExperienceId, x.MatchId });
                    table.ForeignKey(
                        name: "FK_Experience-Reject",
                        column: x => x.ExperienceId,
                        principalTable: "Experiences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Match-Reject",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_MatchId",
                table: "Experiences",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Rejects_MatchId",
                table: "Rejects",
                column: "MatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Experience-Match",
                table: "Experiences",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experience-Match",
                table: "Experiences");

            migrationBuilder.DropTable(
                name: "Rejects");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Experiences_MatchId",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "MatchId",
                table: "Experiences");
        }
    }
}
