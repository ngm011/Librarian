using Microsoft.EntityFrameworkCore.Migrations;

namespace Librarian.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CatalogUsagePrints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginatingIP = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    OriginatingHost = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Query = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogUsagePrints", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogUsagePrints");
        }
    }
}
