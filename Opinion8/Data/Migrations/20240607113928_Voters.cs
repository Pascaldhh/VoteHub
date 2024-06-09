using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Opinion8.Data.Migrations
{
    /// <inheritdoc />
    public partial class Voters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalVoters",
                table: "Polls",
                newName: "Voters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Voters",
                table: "Polls",
                newName: "TotalVoters");
        }
    }
}
