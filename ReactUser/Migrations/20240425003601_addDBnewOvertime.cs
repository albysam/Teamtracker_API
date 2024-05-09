using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactUser.Migrations
{
    /// <inheritdoc />
    public partial class addDBnewOvertime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserOvertimes_UserShifts_UserShiftId",
                table: "UserOvertimes");

            migrationBuilder.DropIndex(
                name: "IX_UserOvertimes_UserShiftId",
                table: "UserOvertimes");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "UserOvertimes");

            migrationBuilder.DropColumn(
                name: "UserShiftId",
                table: "UserOvertimes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "UserOvertimes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserShiftId",
                table: "UserOvertimes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserOvertimes_UserShiftId",
                table: "UserOvertimes",
                column: "UserShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserOvertimes_UserShifts_UserShiftId",
                table: "UserOvertimes",
                column: "UserShiftId",
                principalTable: "UserShifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
