using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBulkMealsApp.Migrations
{
    public partial class RestoreRecipeIng_Oct13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserItem_UserItem_RecipeId",
                table: "UserItem");

            migrationBuilder.DropIndex(
                name: "IX_UserItem_RecipeId",
                table: "UserItem");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "UserItem");

            migrationBuilder.CreateTable(
                name: "RecipeIngredient",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IngredientId = table.Column<int>(nullable: false),
                    RecipeId = table.Column<int>(nullable: false),
                    MeasurementAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeIngredient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeIngredient_UserItem_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "UserItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RecipeIngredient_UserItem_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "UserItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredient_IngredientId",
                table: "RecipeIngredient",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredient_RecipeId",
                table: "RecipeIngredient",
                column: "RecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeIngredient");

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "UserItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserItem_RecipeId",
                table: "UserItem",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserItem_UserItem_RecipeId",
                table: "UserItem",
                column: "RecipeId",
                principalTable: "UserItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
