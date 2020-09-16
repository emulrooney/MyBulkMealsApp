using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBulkMealsApp.Data.Migrations
{
    public partial class Sept15_DBUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateTable(
                name: "Ingredient",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ItemName = table.Column<string>(unicode: false, maxLength: 128, nullable: false),
                    MeasurementType = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    BaseMeasurement = table.Column<double>(nullable: false),
                    Calories = table.Column<short>(nullable: true),
                    Protein = table.Column<short>(nullable: true),
                    Carbs = table.Column<short>(nullable: true),
                    Fat = table.Column<short>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredient", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealPlan",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    PlanName = table.Column<string>(unicode: false, maxLength: 128, nullable: false),
                    CreatorId = table.Column<int>(nullable: false),
                    MealsPerDay = table.Column<byte>(nullable: false),
                    TotalDays = table.Column<byte>(nullable: false),
                    StartDay = table.Column<byte>(nullable: false),
                    EndDay = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPlan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealPlanEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    IsException = table.Column<byte[]>(fixedLength: true, maxLength: 1, nullable: true),
                    Quantity = table.Column<byte>(nullable: false),
                    MealPlanId = table.Column<int>(nullable: false),
                    RecipeId = table.Column<int>(nullable: false),
                    Day = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPlanEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipe",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ItemName = table.Column<string>(unicode: false, maxLength: 128, nullable: false),
                    ImageUrl = table.Column<string>(unicode: false, maxLength: 256, nullable: true),
                    BaseServings = table.Column<byte>(nullable: false),
                    Instructions = table.Column<string>(unicode: false, maxLength: 4096, nullable: true),
                    Step = table.Column<string>(unicode: false, maxLength: 4096, nullable: true),
                    Time = table.Column<byte>(nullable: true),
                    Views = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipeIngredient",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    IngredientId = table.Column<int>(nullable: false),
                    RecipeId = table.Column<int>(nullable: false),
                    MeasurementAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeIngredient", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ItemName = table.Column<string>(unicode: false, maxLength: 128, nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatorId = table.Column<int>(nullable: false),
                    CopyOf = table.Column<int>(nullable: true),
                    IsVerified = table.Column<byte[]>(fixedLength: true, maxLength: 1, nullable: true),
                    IsPublic = table.Column<byte[]>(fixedLength: true, maxLength: 1, nullable: true),
                    VerificationSubmission = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsAmendment = table.Column<byte[]>(fixedLength: true, maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSavedItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    UserItemId = table.Column<int>(nullable: false),
                    SavedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSavedItem", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "([NormalizedUserName] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "([NormalizedName] IS NOT NULL)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ingredient");

            migrationBuilder.DropTable(
                name: "MealPlan");

            migrationBuilder.DropTable(
                name: "MealPlanEntry");

            migrationBuilder.DropTable(
                name: "Recipe");

            migrationBuilder.DropTable(
                name: "RecipeIngredient");

            migrationBuilder.DropTable(
                name: "UserItem");

            migrationBuilder.DropTable(
                name: "UserSavedItem");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");
        }
    }
}
