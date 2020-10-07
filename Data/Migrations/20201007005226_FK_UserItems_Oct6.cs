using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBulkMealsApp.Data.Migrations
{
    public partial class FK_UserItems_Oct6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserItem_IngredientId",
                table: "UserItem",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_UserItem_RecipeId",
                table: "UserItem",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserItem_Ingredient_IngredientId",
                table: "UserItem",
                column: "IngredientId",
                principalTable: "Ingredient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserItem_Recipe_RecipeId",
                table: "UserItem",
                column: "RecipeId",
                principalTable: "Recipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserItem_Ingredient_IngredientId",
                table: "UserItem");

            migrationBuilder.DropForeignKey(
                name: "FK_UserItem_Recipe_RecipeId",
                table: "UserItem");

            migrationBuilder.DropIndex(
                name: "IX_UserItem_IngredientId",
                table: "UserItem");

            migrationBuilder.DropIndex(
                name: "IX_UserItem_RecipeId",
                table: "UserItem");
        }
    }
}
