using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductApiExample.DataLayer.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    ImgUri = table.Column<string>(unicode: false, nullable: false),
                    Price = table.Column<decimal>(type: "decimal", nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            InsertProduct("Car", "https://ourdomain.com/productImages/car.jpg", 250000, "An ordinary car.");
            InsertProduct("Camera", "https://ourdomain.com/productImages/camera.jpg", 5000, "Compact camera with posibility of advanced settings.");
            InsertProduct("Chardonay", "https://ourdomain.com/productImages/chardonay.jpg", 350, "High quality vine.");
            InsertProduct("Teddy bear", "https://ourdomain.com/productImages/teddybear.jpg", 200, "Must-have for every child.");

            void InsertProduct(string name, string imgUri, decimal price, string? description)
            {
                migrationBuilder.InsertData("Products", new string[] { "Name", "ImgUri", "Price", "Description" }, new object?[] { name, imgUri, price, description });
            }            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
