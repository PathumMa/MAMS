using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAMS.API.Migrations
{
    /// <inheritdoc />
    public partial class updateSpecRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorDetails_Specializations_Specializations_Id",
                table: "DoctorDetails");

            migrationBuilder.DropIndex(
                name: "IX_DoctorDetails_Specializations_Id",
                table: "DoctorDetails");

            migrationBuilder.DropColumn(
                name: "Specializations_Id",
                table: "DoctorDetails");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorDetails_Specialization_Id",
                table: "DoctorDetails",
                column: "Specialization_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorDetails_Specializations_Specialization_Id",
                table: "DoctorDetails",
                column: "Specialization_Id",
                principalTable: "Specializations",
                principalColumn: "Specializations_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorDetails_Specializations_Specialization_Id",
                table: "DoctorDetails");

            migrationBuilder.DropIndex(
                name: "IX_DoctorDetails_Specialization_Id",
                table: "DoctorDetails");

            migrationBuilder.AddColumn<int>(
                name: "Specializations_Id",
                table: "DoctorDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DoctorDetails_Specializations_Id",
                table: "DoctorDetails",
                column: "Specializations_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorDetails_Specializations_Specializations_Id",
                table: "DoctorDetails",
                column: "Specializations_Id",
                principalTable: "Specializations",
                principalColumn: "Specializations_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
