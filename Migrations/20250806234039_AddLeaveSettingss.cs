using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CimtasHrPanel.Migrations
{
    /// <inheritdoc />
    public partial class AddLeaveSettingss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "PersonLastName", "PersonName" },
                values: new object[] { "Uyar", "Doğukan Sezer" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "PersonLastName", "PersonName" },
                values: new object[] { "Yıldız", "Ayşe" });
        }
    }
}
