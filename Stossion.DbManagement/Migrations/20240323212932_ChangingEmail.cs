using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stossion.DbManagement.Migrations
{
    /// <inheritdoc />
    public partial class ChangingEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChangingEmail",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmailChangeConfirmationToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangingEmail",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmailChangeConfirmationToken",
                table: "AspNetUsers");
        }
    }
}
