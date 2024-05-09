using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactUser.Migrations
{
    /// <inheritdoc />
    public partial class updateUserLeaveTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "UserLeaves",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFrom",
                table: "UserLeaves",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTo",
                table: "UserLeaves",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "UserLeaves",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "UserLeaves");

            migrationBuilder.DropColumn(
                name: "DateFrom",
                table: "UserLeaves");

            migrationBuilder.DropColumn(
                name: "DateTo",
                table: "UserLeaves");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "UserLeaves");
        }
    }
}
