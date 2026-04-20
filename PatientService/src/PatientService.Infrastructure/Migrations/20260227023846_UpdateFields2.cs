using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFields2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "Customer",
                table: "CustomerAddress",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "Customer",
                table: "CustomerAddress",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "Customer",
                table: "Customer",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "Customer",
                table: "Customer",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "Customer",
                table: "CustomerAddress");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Customer",
                table: "CustomerAddress");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "Customer",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Customer",
                table: "Customer");
        }
    }
}
