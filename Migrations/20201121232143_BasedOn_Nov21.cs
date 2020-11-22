using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBulkMealsApp.Migrations
{
    public partial class BasedOn_Nov21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BasedOn",
                table: "UserItem",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasedOn",
                table: "UserItem");
        }
    }
}
