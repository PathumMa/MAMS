using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAMS.API.Migrations
{
    /// <inheritdoc />
    public partial class appoinmentsAndDoctorRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DoctorDetails_DoctorDetailsId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorDetailsId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "DoctorDetailsId",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Doctor_Id",
                table: "Appointments",
                column: "Doctor_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_DoctorDetails_Doctor_Id",
                table: "Appointments",
                column: "Doctor_Id",
                principalTable: "DoctorDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DoctorDetails_Doctor_Id",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_Doctor_Id",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "DoctorDetailsId",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorDetailsId",
                table: "Appointments",
                column: "DoctorDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_DoctorDetails_DoctorDetailsId",
                table: "Appointments",
                column: "DoctorDetailsId",
                principalTable: "DoctorDetails",
                principalColumn: "Id");
        }
    }
}
