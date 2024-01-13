using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAMS.API.Migrations
{
    /// <inheritdoc />
    public partial class docAvailable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DoctorDetails_SuserDetailsId",
                table: "DoctorDetails");

            migrationBuilder.DropColumn(
                name: "AvailableDay",
                table: "DoctorDetails");

            migrationBuilder.DropColumn(
                name: "AvailableTime",
                table: "DoctorDetails");

            migrationBuilder.CreateTable(
                name: "DoctorAvailableDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuserDetailsId = table.Column<int>(type: "int", nullable: false),
                    AvailableDay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvailableTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorAvailableDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorAvailableDetails_SuserDetails_SuserDetailsId",
                        column: x => x.SuserDetailsId,
                        principalTable: "SuserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorDetails_SuserDetailsId",
                table: "DoctorDetails",
                column: "SuserDetailsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DoctorAvailableDetails_SuserDetailsId",
                table: "DoctorAvailableDetails",
                column: "SuserDetailsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorAvailableDetails");

            migrationBuilder.DropIndex(
                name: "IX_DoctorDetails_SuserDetailsId",
                table: "DoctorDetails");

            migrationBuilder.AddColumn<string>(
                name: "AvailableDay",
                table: "DoctorDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AvailableTime",
                table: "DoctorDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorDetails_SuserDetailsId",
                table: "DoctorDetails",
                column: "SuserDetailsId");
        }
    }
}
