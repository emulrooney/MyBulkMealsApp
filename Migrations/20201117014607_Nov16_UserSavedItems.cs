using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBulkMealsApp.Migrations
{
    public partial class Nov16_UserSavedItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SavedBy",
                table: "UserSavedItem",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_UserSavedItem_UserItemId",
                table: "UserSavedItem",
                column: "UserItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavedItem_UserItem_UserItemId",
                table: "UserSavedItem",
                column: "UserItemId",
                principalTable: "UserItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSavedItem_UserItem_UserItemId",
                table: "UserSavedItem");

            migrationBuilder.DropIndex(
                name: "IX_UserSavedItem_UserItemId",
                table: "UserSavedItem");

            migrationBuilder.AlterColumn<int>(
                name: "SavedBy",
                table: "UserSavedItem",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
