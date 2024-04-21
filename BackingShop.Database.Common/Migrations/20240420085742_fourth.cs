using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackingShop.Database.Common.Migrations
{
    /// <inheritdoc />
    public partial class fourth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title_Value",
                schema: "dbo",
                table: "products",
                newName: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                schema: "dbo",
                table: "products",
                newName: "Title_Value");
        }
    }
}
