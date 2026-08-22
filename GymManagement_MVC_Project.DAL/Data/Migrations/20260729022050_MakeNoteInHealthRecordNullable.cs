using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagement_MVC_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeNoteInHealthRecordNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_SessionId_MemberId",
                table: "Bookings");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "HealthRecords",
                type: "VarChar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SessionId_MemberId",
                table: "Bookings",
                columns: new[] { "SessionId", "MemberId" },
                unique: true,
                filter: "IsDeleted = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_SessionId_MemberId",
                table: "Bookings");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "HealthRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VarChar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SessionId_MemberId",
                table: "Bookings",
                columns: new[] { "SessionId", "MemberId" },
                unique: true);
        }
    }
}
