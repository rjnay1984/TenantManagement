using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class UseBaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitId",
                table: "Units",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "BuildingId",
                table: "Buildings",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Units",
                newName: "UnitId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Buildings",
                newName: "BuildingId");
        }
    }
}
