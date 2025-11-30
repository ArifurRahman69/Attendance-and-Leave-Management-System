using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance_and_Leave_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class akash2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeID",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeID",
                table: "Employees");
        }
    }
}
