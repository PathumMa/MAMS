using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAMS.API.Migrations
{
    /// <inheritdoc />
    public partial class MakeAppointmentIdNullable_And_RestrictLabSlotBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Appointments_Appointment_Id",
                table: "Transactions");

            migrationBuilder.CreateIndex(
                name: "IX_LabResults_PatientId_BookedDate_TimeSlot",
                table: "LabResults",
                columns: new[] { "PatientId", "BookedDate", "TimeSlot" },
                unique: true,
                filter: "[TimeSlot] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Appointments_Appointment_Id",
                table: "Transactions",
                column: "Appointment_Id",
                principalTable: "Appointments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Appointments_Appointment_Id",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_LabResults_PatientId_BookedDate_TimeSlot",
                table: "LabResults");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Appointments_Appointment_Id",
                table: "Transactions",
                column: "Appointment_Id",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
