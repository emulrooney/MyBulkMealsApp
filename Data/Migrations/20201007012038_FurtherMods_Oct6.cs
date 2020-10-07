using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBulkMealsApp.Data.Migrations
{
    public partial class FurtherMods_Oct6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserItem_Ingredient_IngredientId",
                table: "UserItem");

            migrationBuilder.DropForeignKey(
                name: "FK_UserItem_Recipe_RecipeId",
                table: "UserItem");

            migrationBuilder.DropTable(
                name: "RecipeIngredient");

            migrationBuilder.DropTable(
                name: "Ingredient");

            migrationBuilder.DropTable(
                name: "Recipe");

            migrationBuilder.DropIndex(
                name: "IX_UserItem_IngredientId",
                table: "UserItem");

            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "UserItem");

            migrationBuilder.RenameColumn(
                name: "VerificationSubmission",
                table: "UserItem",
                newName: "VerificationSubmissionTime");

            migrationBuilder.AlterColumn<bool>(
                name: "IsVerified",
                table: "UserItem",
                fixedLength: true,
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "binary(1)",
                oldFixedLength: true,
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublic",
                table: "UserItem",
                fixedLength: true,
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "binary(1)",
                oldFixedLength: true,
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsAmendment",
                table: "UserItem",
                fixedLength: true,
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "binary(1)",
                oldFixedLength: true,
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BaseMeasurement",
                table: "UserItem",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Calories",
                table: "UserItem",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Carbs",
                table: "UserItem",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Fat",
                table: "UserItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeasurementType",
                table: "UserItem",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Protein",
                table: "UserItem",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BaseServings",
                table: "UserItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "UserItem",
                unicode: false,
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instructions",
                table: "UserItem",
                unicode: false,
                maxLength: 4096,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Time",
                table: "UserItem",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "UserItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "UserItem",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "TotalDays",
                table: "MealPlan",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<int>(
                name: "StartDay",
                table: "MealPlan",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<int>(
                name: "MealsPerDay",
                table: "MealPlan",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<int>(
                name: "EndDay",
                table: "MealPlan",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AddForeignKey(
                name: "FK_UserItem_UserItem_RecipeId",
                table: "UserItem",
                column: "RecipeId",
                principalTable: "UserItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserItem_UserItem_RecipeId",
                table: "UserItem");

            migrationBuilder.DropColumn(
                name: "BaseMeasurement",
                table: "UserItem");

            migrationBuilder.DropColumn(
                name: "Calories",
                table: "UserItem");

            migrationBuilder.DropColumn(
                name: "Carbs",
                table: "UserItem");

            migrationBuilder.DropColumn(
                name: "Fat",
                table: "UserItem");

            migrationBuilder.DropColumn(
                name: "MeasurementType",
                table: "UserItem");

            migrationBuilder.DropColumn(
                name: "Protein",
                table: "UserItem");

            migrationBuilder.DropColumn(
                name: "BaseServings",
                table: "UserItem");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "UserItem");

            migrationBuilder.DropColumn(
                name: "Instructions",
                table: "UserItem");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "UserItem");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "UserItem");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "UserItem");

            migrationBuilder.RenameColumn(
                name: "VerificationSubmissionTime",
                table: "UserItem",
                newName: "VerificationSubmission");

            migrationBuilder.AlterColumn<byte[]>(
                name: "IsVerified",
                table: "UserItem",
                type: "binary(1)",
                fixedLength: true,
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(bool),
                oldFixedLength: true,
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<byte[]>(
                name: "IsPublic",
                table: "UserItem",
                type: "binary(1)",
                fixedLength: true,
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(bool),
                oldFixedLength: true,
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<byte[]>(
                name: "IsAmendment",
                table: "UserItem",
                type: "binary(1)",
                fixedLength: true,
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(bool),
                oldFixedLength: true,
                oldMaxLength: 1);

            migrationBuilder.AddColumn<int>(
                name: "IngredientId",
                table: "UserItem",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "TotalDays",
                table: "MealPlan",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<byte>(
                name: "StartDay",
                table: "MealPlan",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<byte>(
                name: "MealsPerDay",
                table: "MealPlan",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<byte>(
                name: "EndDay",
                table: "MealPlan",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "Ingredient",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BaseMeasurement = table.Column<double>(type: "float", nullable: false),
                    Calories = table.Column<short>(type: "smallint", nullable: true),
                    Carbs = table.Column<short>(type: "smallint", nullable: true),
                    Fat = table.Column<short>(type: "smallint", nullable: true),
                    ItemName = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    MeasurementType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Protein = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredient", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BaseServings = table.Column<byte>(type: "tinyint", nullable: false),
                    ImageUrl = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: true),
                    Instructions = table.Column<string>(type: "varchar(4096)", unicode: false, maxLength: 4096, nullable: true),
                    ItemName = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    Time = table.Column<byte>(type: "tinyint", nullable: true),
                    Views = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipeIngredient",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    IngredientId = table.Column<int>(type: "int", nullable: false),
                    MeasurementAmount = table.Column<double>(type: "float", nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeIngredient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeIngredient_Ingredient_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeIngredient_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserItem_IngredientId",
                table: "UserItem",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredient_IngredientId",
                table: "RecipeIngredient",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredient_RecipeId",
                table: "RecipeIngredient",
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
    }
}
