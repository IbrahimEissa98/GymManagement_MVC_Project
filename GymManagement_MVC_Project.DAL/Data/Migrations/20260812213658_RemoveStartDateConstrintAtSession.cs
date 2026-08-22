using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagement_MVC_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStartDateConstrintAtSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Session_StartDate",
                table: "Sessions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Session_StartDate",
                table: "Sessions",
                sql: "StartDate > SYSUTCDATETIME()");
        }
    }
}
