using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAMS.API.Migrations
{
    /// <inheritdoc />
    public partial class cleanAppointments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DoctorDetails_Doctor_Id",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientDetails_Appointments_Appointment_Id",
                table: "PatientDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Appointments_Appointment_Id",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_Appointment_Id",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_PatientDetails_Appointment_Id",
                table: "PatientDetails");

            migrationBuilder.DropColumn(
                name: "Appointment_Id",
                table: "PatientDetails");

            migrationBuilder.AlterColumn<int>(
                name: "Appointment_Id",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatientDetails_Id",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Appointment_Id",
                table: "Transactions",
                column: "Appointment_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientDetails_Id_Doctor_Id_Appointment_Date",
                table: "Appointments",
                columns: new[] { "PatientDetails_Id", "Doctor_Id", "Appointment_Date" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_DoctorDetails_Doctor_Id",
                table: "Appointments",
                column: "Doctor_Id",
                principalTable: "DoctorDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_PatientDetails_PatientDetails_Id",
                table: "Appointments",
                column: "PatientDetails_Id",
                principalTable: "PatientDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Appointments_Appointment_Id",
                table: "Transactions",
                column: "Appointment_Id",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DoctorDetails_Doctor_Id",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_PatientDetails_PatientDetails_Id",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Appointments_Appointment_Id",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_Appointment_Id",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_PatientDetails_Id_Doctor_Id_Appointment_Date",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "PatientDetails_Id",
                table: "Appointments");

            migrationBuilder.AlterColumn<int>(
                name: "Appointment_Id",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Appointment_Id",
                table: "PatientDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Appointment_Id",
                table: "Transactions",
                column: "Appointment_Id",
                unique: true,
                filter: "[Appointment_Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDetails_Appointment_Id",
                table: "PatientDetails",
                column: "Appointment_Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_DoctorDetails_Doctor_Id",
                table: "Appointments",
                column: "Doctor_Id",
                principalTable: "DoctorDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientDetails_Appointments_Appointment_Id",
                table: "PatientDetails",
                column: "Appointment_Id",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Appointments_Appointment_Id",
                table: "Transactions",
                column: "Appointment_Id",
                principalTable: "Appointments",
                principalColumn: "Id");
        }
    }
}
