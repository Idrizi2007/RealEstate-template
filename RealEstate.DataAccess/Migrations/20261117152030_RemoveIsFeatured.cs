using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsFeatured : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Properties");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
