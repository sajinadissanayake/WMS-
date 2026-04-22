using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkforceShiftPorta.Migrations
{
    /// <inheritdoc />
    public partial class AddDateToShift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "Shifts");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Shifts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Shifts");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Shifts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
