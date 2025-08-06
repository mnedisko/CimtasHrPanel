using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CimtasHrPanel.Migrations
{
    /// <inheritdoc />
    public partial class AddDStatusd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LeaveRequests",
                columns: new[] { "Id", "DepartmentId", "DurationDays", "EntryTime", "LeaveTime", "LeaveTypeId", "PersonId", "status" },
                values: new object[] { 2, null, 22, new DateTime(2025, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
