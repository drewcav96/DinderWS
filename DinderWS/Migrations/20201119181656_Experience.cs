using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DinderWS.Migrations
{
    public partial class Experience : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Experiences",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    CuisineType = table.Column<int>(nullable: false),
                    GroupSize = table.Column<byte>(nullable: false),
                    GenderPreference = table.Column<byte>(nullable: false),
                    LocationLong = table.Column<double>(nullable: false),
                    LocationLat = table.Column<double>(nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profile-Experience",
                        column: x => x.Id,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Identity-Experience",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Experiences");
        }
    }
}
