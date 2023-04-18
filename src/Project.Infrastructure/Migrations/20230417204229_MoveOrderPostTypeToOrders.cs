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
                nullable: true);

            migrationBuilder.Sql(@"UPDATE ""Orders"" AS O SET ""PostType"" = 
                                    (CASE 
                                          WHEN (SELECT count(*) FROM ""Goods"" AS G 
						                                    JOIN   ""OrderItems"" AS OI ON OI.""GoodId"" = G.""Id"" 
						                                    WHERE OI.""OrderId"" = O.""Id"" AND G.""IsFragile"" ) > 0  THEN 'SpecialPost'
                                          ELSE 'OrdinaryPost'
                                    END)");

            migrationBuilder.AlterColumn<string>(
                name: "PostType",
                table: "Orders",
                type: "text",
                nullable: false);
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
