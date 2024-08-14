using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vireo.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangePhoneSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Clients",
                type: "varchar(13)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Clients",
                type: "varchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(13)");
        }
    }
}
