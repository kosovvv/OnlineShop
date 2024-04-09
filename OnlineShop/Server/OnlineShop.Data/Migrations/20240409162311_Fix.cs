using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserProduct");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProductId",
                table: "AspNetUsers",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Products_ProductId",
                table: "AspNetUsers",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Products_ProductId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProductId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ApplicationUserProduct",
                columns: table => new
                {
                    FavouriteProductsId = table.Column<int>(type: "int", nullable: false),
                    UsersFavouringTheProductId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserProduct", x => new { x.FavouriteProductsId, x.UsersFavouringTheProductId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserProduct_AspNetUsers_UsersFavouringTheProductId",
                        column: x => x.UsersFavouringTheProductId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserProduct_Products_FavouriteProductsId",
                        column: x => x.FavouriteProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserProduct_UsersFavouringTheProductId",
                table: "ApplicationUserProduct",
                column: "UsersFavouringTheProductId");
        }
    }
}
