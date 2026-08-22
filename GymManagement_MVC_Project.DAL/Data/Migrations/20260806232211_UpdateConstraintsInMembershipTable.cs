using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagement_MVC_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConstraintsInMembershipTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Membership_StartDate",
                table: "Memberships");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Membership_StartDate",
                table: "Memberships",
                sql: "StartDate >= GetDate()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Membership_StartDate",
                table: "Memberships");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Membership_StartDate",
                table: "Memberships",
                sql: "StartDate > GetDate()");
        }
    }
}
