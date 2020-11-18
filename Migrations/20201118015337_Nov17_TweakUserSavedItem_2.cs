using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBulkMealsApp.Migrations
{
    public partial class Nov17_TweakUserSavedItem_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                           name: "IdTwo",
                           table: "UserSavedItem",
                           nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.Sql("ALTER TABLE UserSavedItem DROP CONSTRAINT [PK_UserSavedItem]");

            migrationBuilder.DropColumn(
                           name: "Id",
                           table: "UserSavedItem");

            migrationBuilder.RenameColumn("IdTwo", "UserSavedItem", "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "Id",
               table: "UserSavedItem");

            migrationBuilder.AddColumn<int>(
                           name: "Id",
                           table: "UserSavedItem");
        }
    }
}