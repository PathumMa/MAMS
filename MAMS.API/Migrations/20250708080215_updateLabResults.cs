using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAMS.API.Migrations
{
    /// <inheritdoc />
    public partial class updateLabResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BookedPrice",
                table: "LabResults",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookedPrice",
                table: "LabResults");
        }
    }
}
