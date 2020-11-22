using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBulkMealsApp.Migrations
{
    public partial class AmendmentTweaks_Nov22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AmendmentsCount",
                table: "UserItem",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmendmentsCount",
                table: "UserItem");
        }
    }
}
