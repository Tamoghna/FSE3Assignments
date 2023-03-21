using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductMicroService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BuyerBids",
                columns: table => new
                {
                    BuyerBidId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BuyerFirstName = table.Column<string>(nullable: true),
                    BuyerLastName = table.Column<string>(nullable: true),
                    BuyerAddress = table.Column<string>(nullable: true),
                    BuyerEmail = table.Column<string>(nullable: true),
                    BuyerPhone = table.Column<long>(nullable: false),
                    BuyerCity = table.Column<string>(nullable: true),
                    BuyerState = table.Column<string>(nullable: true),
                    BuyerPin = table.Column<int>(nullable: false),
                    BidPrice = table.Column<int>(nullable: false),
                    BidEndDate = table.Column<DateTime>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyerBids", x => x.BuyerBidId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ProductCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductCategoryName = table.Column<string>(nullable: true),
                    ProductCategoryDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ProductCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductName = table.Column<string>(nullable: true),
                    ProductShortDescription = table.Column<string>(nullable: true),
                    ProductDetailedDescription = table.Column<string>(nullable: true),
                    BidStartingPrice = table.Column<int>(nullable: false),
                    BidEndDate = table.Column<DateTime>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    SellerFirstName = table.Column<string>(nullable: true),
                    SellerLastName = table.Column<string>(nullable: true),
                    SellerAddress = table.Column<string>(nullable: true),
                    SellerEmail = table.Column<string>(nullable: true),
                    SellerPhone = table.Column<long>(nullable: false),
                    SellerCity = table.Column<string>(nullable: true),
                    SellerState = table.Column<string>(nullable: true),
                    SellerPin = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "ProductCategoryId", "ProductCategoryDescription", "ProductCategoryName" },
                values: new object[] { 1, "Painting", "Painting" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "ProductCategoryId", "ProductCategoryDescription", "ProductCategoryName" },
                values: new object[] { 2, "Sculptor", "Sculptor" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "ProductCategoryId", "ProductCategoryDescription", "ProductCategoryName" },
                values: new object[] { 3, "Ornament", "Ornament" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyerBids");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
