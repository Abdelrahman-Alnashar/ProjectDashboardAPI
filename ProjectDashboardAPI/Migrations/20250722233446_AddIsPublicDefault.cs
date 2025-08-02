using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectDashboardAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPublicDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isPublic",
                table: "Projects",
                newName: "IsPublic");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublic",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPublic",
                table: "Projects",
                newName: "isPublic");

            migrationBuilder.AlterColumn<bool>(
                name: "isPublic",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: true);
        }
    }
}
