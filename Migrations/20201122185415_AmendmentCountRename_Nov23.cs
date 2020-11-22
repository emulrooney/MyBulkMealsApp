using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBulkMealsApp.Migrations
{
    public partial class AmendmentCountRename_Nov23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmendmentsCount",
                table: "UserItem");

            migrationBuilder.AddColumn<int>(
                name: "AmendmentCount",
                table: "UserItem",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmendmentCount",
                table: "UserItem");

            migrationBuilder.AddColumn<int>(
                name: "AmendmentsCount",
                table: "UserItem",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
