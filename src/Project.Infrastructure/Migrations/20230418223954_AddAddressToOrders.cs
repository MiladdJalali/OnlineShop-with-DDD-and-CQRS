using Microsoft.EntityFrameworkCore.Migrations;
using Project.Domain.Aggregates.Orders.Enums;

namespace Project.Infrastructure.Migrations
{
    public partial class AddAddressToOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.Sql($@"UPDATE ""Orders"" AS O SET ""Address"" = 
                                         ( SELECT U.""Address"" FROM ""Users"" AS U WHERE U.""Id"" = O.""CreatorId"")
                                   WHERE O.""Status"" != '{OrderStatus.Received}'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Orders");
        }
    }
}
