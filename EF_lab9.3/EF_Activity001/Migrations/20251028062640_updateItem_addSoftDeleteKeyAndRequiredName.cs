using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EF_Activity001.Migrations
{
    /// <inheritdoc />
    public partial class updateItem_addSoftDeleteKeyAndRequiredName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Items");
        }
    }
}
