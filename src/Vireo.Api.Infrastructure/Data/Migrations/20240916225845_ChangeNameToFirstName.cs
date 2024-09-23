using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vireo.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameToFirstName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Clients",
                newName: "FirstName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Clients",
                newName: "Name");
        }
    }
}
