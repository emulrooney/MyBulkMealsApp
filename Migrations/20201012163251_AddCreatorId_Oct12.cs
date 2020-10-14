using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBulkMealsApp.Migrations
{
    public partial class AddCreatorId_Oct12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "UserItem",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "IdentityUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "IdentityUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "IdentityUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "IdentityUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserItem_MeasurementId",
                table: "UserItem",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_UserItem_CreatorId",
                table: "UserItem",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserItem_Measurement_MeasurementId",
                table: "UserItem",
                column: "MeasurementId",
                principalTable: "Measurement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserItem_IdentityUsers_CreatorId",
                table: "UserItem",
                column: "CreatorId",
                principalTable: "IdentityUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserItem_Measurement_MeasurementId",
                table: "UserItem");

            migrationBuilder.DropForeignKey(
                name: "FK_UserItem_IdentityUsers_CreatorId",
                table: "UserItem");

            migrationBuilder.DropIndex(
                name: "IX_UserItem_MeasurementId",
                table: "UserItem");

            migrationBuilder.DropIndex(
                name: "IX_UserItem_CreatorId",
                table: "UserItem");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "IdentityUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "IdentityUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "IdentityUsers");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "IdentityUsers");

            migrationBuilder.AlterColumn<int>(
                name: "CreatorId",
                table: "UserItem",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
