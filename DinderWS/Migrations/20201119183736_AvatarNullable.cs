using Microsoft.EntityFrameworkCore.Migrations;

namespace DinderWS.Migrations
{
    public partial class AvatarNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AvatarUrl",
                table: "Profiles",
                unicode: false,
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(2048)",
                oldUnicode: false,
                oldMaxLength: 2048);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AvatarUrl",
                table: "Profiles",
                type: "varchar(2048)",
                unicode: false,
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 2048,
                oldNullable: true);
        }
    }
}
