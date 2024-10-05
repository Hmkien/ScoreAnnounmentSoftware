using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScoreAnnouncementSoftware.Migrations
{
    /// <inheritdoc />
    public partial class Update_table_ScoreIT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdentificationCode",
                table: "ScoreIT",
                newName: "IdentityNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdentityNumber",
                table: "ScoreIT",
                newName: "IdentificationCode");
        }
    }
}
