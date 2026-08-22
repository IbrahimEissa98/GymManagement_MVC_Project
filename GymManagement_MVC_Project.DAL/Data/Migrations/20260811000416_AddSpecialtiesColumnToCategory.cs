using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagement_MVC_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSpecialtiesColumnToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Specialties",
                table: "Categories",
                type: "VarChar(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Specialties",
                table: "Categories");
        }
    }
}
