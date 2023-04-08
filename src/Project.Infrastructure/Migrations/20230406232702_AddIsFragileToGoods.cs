using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Infrastructure.Migrations
{
    public partial class AddIsFragileToGoods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.Sql("UPDATE \"Users\" SET \"Address\" = 'Address'");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Users",
                type: "text",
                nullable: false);

                migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "Goods",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFragile",
                table: "Goods",
                type: "boolean",
                nullable: true);

            migrationBuilder.Sql("UPDATE \"Goods\" SET \"IsFragile\" = false");

            migrationBuilder.AlterColumn<string>(
                name: "IsFragile",
                table: "Goods",
                type: "boolean",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsFragile",
                table: "Goods");

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "Goods",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }
    }
}
