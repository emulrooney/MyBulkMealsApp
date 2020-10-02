using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBulkMealsApp.Data.Migrations
{
    public partial class UserItemTweaks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CopyOf",
                table: "UserItem");

            migrationBuilder.AddColumn<int>(
                name: "IngredientId",
                table: "UserItem",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "UserItem",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredient_IngredientId",
                table: "RecipeIngredient",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredient_RecipeId",
                table: "RecipeIngredient",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredient_Ingredient_IngredientId",
                table: "RecipeIngredient",
                column: "IngredientId",
                principalTable: "Ingredient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredient_Recipe_RecipeId",
                table: "RecipeIngredient",
                column: "RecipeId",
                principalTable: "Recipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredient_Ingredient_IngredientId",
                table: "RecipeIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredient_Recipe_RecipeId",
                table: "RecipeIngredient");

            migrationBuilder.DropIndex(
                name: "IX_RecipeIngredient_IngredientId",
                table: "RecipeIngredient");

            migrationBuilder.DropIndex(
                name: "IX_RecipeIngredient_RecipeId",
                table: "RecipeIngredient");

            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "UserItem");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "UserItem");

            migrationBuilder.AddColumn<int>(
                name: "CopyOf",
                table: "UserItem",
                type: "int",
                nullable: true);
        }
    }
}
