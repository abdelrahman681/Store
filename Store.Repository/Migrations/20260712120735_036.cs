using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Repository.Migrations
{
    /// <inheritdoc />
    public partial class _036 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WishListItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WishListCustomerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WishListProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishListItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WishListItem_WishList_WishListCustomerId_WishListProductId",
                        columns: x => new { x.WishListCustomerId, x.WishListProductId },
                        principalTable: "WishList",
                        principalColumns: new[] { "CustomerId", "ProductId" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_WishListItem_WishListCustomerId_WishListProductId",
                table: "WishListItem",
                columns: new[] { "WishListCustomerId", "WishListProductId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WishListItem");
        }
    }
}
