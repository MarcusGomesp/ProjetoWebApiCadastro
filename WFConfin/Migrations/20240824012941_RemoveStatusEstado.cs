using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WFConfin.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStatusEstado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Estado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Estado",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
