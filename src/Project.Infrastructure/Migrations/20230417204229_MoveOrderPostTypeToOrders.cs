using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Infrastructure.Migrations
{
    public partial class MoveOrderPostTypeToOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderPostType",
                table: "OrderItems");

            migrationBuilder.AddColumn<string>(
                name: "PostType",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostType",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "OrderPostType",
                table: "OrderItems",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
