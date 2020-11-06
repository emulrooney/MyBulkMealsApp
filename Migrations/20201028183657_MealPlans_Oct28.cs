using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBulkMealsApp.Migrations
{
    public partial class MealPlans_Oct28 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MealPlanId",
                table: "MealPlanEntry",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_MealPlanEntry_MealPlanId",
                table: "MealPlanEntry",
                column: "MealPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_MealPlanEntry_RecipeId",
                table: "MealPlanEntry",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlanEntry_MealPlan_MealPlanId",
                table: "MealPlanEntry",
                column: "MealPlanId",
                principalTable: "MealPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlanEntry_UserItem_RecipeId",
                table: "MealPlanEntry",
                column: "RecipeId",
                principalTable: "UserItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealPlanEntry_MealPlan_MealPlanId",
                table: "MealPlanEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_MealPlanEntry_UserItem_RecipeId",
                table: "MealPlanEntry");

            migrationBuilder.DropIndex(
                name: "IX_MealPlanEntry_MealPlanId",
                table: "MealPlanEntry");

            migrationBuilder.DropIndex(
                name: "IX_MealPlanEntry_RecipeId",
                table: "MealPlanEntry");

            migrationBuilder.AlterColumn<int>(
                name: "MealPlanId",
                table: "MealPlanEntry",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
