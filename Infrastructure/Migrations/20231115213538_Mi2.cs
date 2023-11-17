using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Mi2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favourite_Products_ProductID",
                table: "Favourite");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_Products_ProductID",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdersDetails_Products_ProductID",
                table: "OrdersDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_ProductID",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Products_ProductID",
                table: "ShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_Warrantys_Products_ProductID",
                table: "Warrantys");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Products_ProductID",
                table: "Wishlists");

            migrationBuilder.AddForeignKey(
                name: "FK_Favourite_Products_ProductID",
                table: "Favourite",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Products_ProductID",
                table: "Image",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersDetails_Products_ProductID",
                table: "OrdersDetails",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_ProductID",
                table: "Reviews",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Products_ProductID",
                table: "ShoppingCarts",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Warrantys_Products_ProductID",
                table: "Warrantys",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Products_ProductID",
                table: "Wishlists",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favourite_Products_ProductID",
                table: "Favourite");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_Products_ProductID",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdersDetails_Products_ProductID",
                table: "OrdersDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_ProductID",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Products_ProductID",
                table: "ShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_Warrantys_Products_ProductID",
                table: "Warrantys");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Products_ProductID",
                table: "Wishlists");

            migrationBuilder.AddForeignKey(
                name: "FK_Favourite_Products_ProductID",
                table: "Favourite",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Products_ProductID",
                table: "Image",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersDetails_Products_ProductID",
                table: "OrdersDetails",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_ProductID",
                table: "Reviews",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Products_ProductID",
                table: "ShoppingCarts",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Warrantys_Products_ProductID",
                table: "Warrantys",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Products_ProductID",
                table: "Wishlists",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
