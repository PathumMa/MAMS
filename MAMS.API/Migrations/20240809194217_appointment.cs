using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAMS.API.Migrations
{
    /// <inheritdoc />
    public partial class appointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DoctorAvailableDetails_DoctorAvailableDetailsId",
                table: "Appointments");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorAvailableDetailsId",
                table: "Appointments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_DoctorAvailableDetails_DoctorAvailableDetailsId",
                table: "Appointments",
                column: "DoctorAvailableDetailsId",
                principalTable: "DoctorAvailableDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DoctorAvailableDetails_DoctorAvailableDetailsId",
                table: "Appointments");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorAvailableDetailsId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_DoctorAvailableDetails_DoctorAvailableDetailsId",
                table: "Appointments",
                column: "DoctorAvailableDetailsId",
                principalTable: "DoctorAvailableDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
