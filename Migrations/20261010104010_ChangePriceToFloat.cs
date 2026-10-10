using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace demo1.Migrations
{
    /// <inheritdoc />
    public partial class ChangePriceToFloat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
            name: "Price",
            table: "Products",
            type: "real", 
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
            name: "Price",
            table: "Products",
            type: "real",
            nullable: false,
            oldClrType: typeof(float),
            oldType: "float");
        }
    }
}
