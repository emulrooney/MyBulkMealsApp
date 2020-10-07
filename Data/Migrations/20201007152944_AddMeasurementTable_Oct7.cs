using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBulkMealsApp.Data.Migrations
{
    public partial class AddMeasurementTable_Oct7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeasurementType",
                table: "UserItem");

            migrationBuilder.AddColumn<int>(
                name: "MeasurementId",
                table: "UserItem",
                unicode: false,
                nullable: true);

            migrationBuilder.CreateTable(
            name: "Measurement",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(nullable: true),
                Symbol = table.Column<string>(nullable: true)
            });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeasurementId",
                table: "UserItem");

            migrationBuilder.AddColumn<string>(
                name: "MeasurementType",
                table: "UserItem",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.DropTable(
               name: "Measurement");
        }
    }
}
