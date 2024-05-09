using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactUser.Migrations
{
    /// <inheritdoc />
    public partial class updateShiftDBRevert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TotalShiftDuration",
                table: "Shifts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalShiftDuration",
                table: "Shifts");
        }
    }
}
