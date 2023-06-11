using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeArrivalData.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToArrivalDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
               name: "IX_EmployeeArrivals_ArrivalDateTime",
               table: "EmployeeArrivals",
               column: "ArrivalDateTime",
               unique: false
           );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
               name: "IX_EmployeeArrivals_ArrivalDateTime",
               table: "EmployeeArrivals"
           );
        }
    }
}
