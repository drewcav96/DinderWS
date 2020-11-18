using Microsoft.EntityFrameworkCore.Migrations;

namespace DinderWS.Migrations
{
    public partial class Profile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetProfiles",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Firstname = table.Column<string>(maxLength: 35, nullable: false),
                    Lastname = table.Column<string>(maxLength: 35, nullable: false),
                    Gender = table.Column<byte>(nullable: false),
                    AvatarUrl = table.Column<string>(unicode: false, maxLength: 2048, nullable: false),
                    DietaryRestrictions = table.Column<int>(nullable: false),
                    Interests = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identity-Profile",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetProfiles");
        }
    }
}
