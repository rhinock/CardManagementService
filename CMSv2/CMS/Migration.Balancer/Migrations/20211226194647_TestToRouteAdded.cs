using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Balancer.Migrations
{
    public partial class TestToRouteAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Test",
                table: "Route",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test",
                table: "Route");
        }
    }
}
