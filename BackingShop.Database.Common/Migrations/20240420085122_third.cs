using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackingShop.Database.Common.Migrations
{
    /// <inheritdoc />
    public partial class third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                schema: "dbo",
                table: "products",
                newName: "Title_Value");

            migrationBuilder.AddColumn<Guid>(
                name: "Name_ProductId",
                schema: "dbo",
                table: "personalEvents",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Name_ProductId",
                schema: "dbo",
                table: "groupEvents",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name_ProductId",
                schema: "dbo",
                table: "personalEvents");

            migrationBuilder.DropColumn(
                name: "Name_ProductId",
                schema: "dbo",
                table: "groupEvents");

            migrationBuilder.RenameColumn(
                name: "Title_Value",
                schema: "dbo",
                table: "products",
                newName: "Title");
        }
    }
}
