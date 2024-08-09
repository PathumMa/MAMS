using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAMS.API.Migrations
{
    /// <inheritdoc />
    public partial class updateDataContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_PatientDetails_PatientDetailsId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Appointments_AppointmentId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_UserDetails_User_PersonalId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_AppointmentId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "User_PersonalId",
                table: "Transactions",
                newName: "Appointment_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_User_PersonalId",
                table: "Transactions",
                newName: "IX_Transactions_Appointment_Id");

            migrationBuilder.RenameColumn(
                name: "PatientDetailsId",
                table: "Appointments",
                newName: "DoctorAvailableDetailsId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_PatientDetailsId",
                table: "Appointments",
                newName: "IX_Appointments_DoctorAvailableDetailsId");

            migrationBuilder.AddColumn<int>(
                name: "TransactionsId",
                table: "UserDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatientDetailsId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "PatientDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "PatientDetails",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "Appointment_Id",
                table: "PatientDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "User_PersonalId",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Availability_Id",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_TransactionsId",
                table: "UserDetails",
                column: "TransactionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PatientDetailsId",
                table: "Transactions",
                column: "PatientDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDetails_Appointment_Id",
                table: "PatientDetails",
                column: "Appointment_Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_DoctorAvailableDetails_DoctorAvailableDetailsId",
                table: "Appointments",
                column: "DoctorAvailableDetailsId",
                principalTable: "DoctorAvailableDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_PatientDetails_PatientDetailsId",
                table: "Transactions",
                column: "PatientDetailsId",
                principalTable: "PatientDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetails_Transactions_TransactionsId",
                table: "UserDetails",
                column: "TransactionsId",
                principalTable: "Transactions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DoctorAvailableDetails_DoctorAvailableDetailsId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientDetails_Appointments_Appointment_Id",
                table: "PatientDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Appointments_Appointment_Id",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_PatientDetails_PatientDetailsId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDetails_Transactions_TransactionsId",
                table: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserDetails_TransactionsId",
                table: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_PatientDetailsId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_PatientDetails_Appointment_Id",
                table: "PatientDetails");

            migrationBuilder.DropColumn(
                name: "TransactionsId",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "PatientDetailsId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Appointment_Id",
                table: "PatientDetails");

            migrationBuilder.DropColumn(
                name: "Availability_Id",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "Appointment_Id",
                table: "Transactions",
                newName: "User_PersonalId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_Appointment_Id",
                table: "Transactions",
                newName: "IX_Transactions_User_PersonalId");

            migrationBuilder.RenameColumn(
                name: "DoctorAvailableDetailsId",
                table: "Appointments",
                newName: "PatientDetailsId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_DoctorAvailableDetailsId",
                table: "Appointments",
                newName: "IX_Appointments_PatientDetailsId");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "PatientDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "PatientDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "User_PersonalId",
                table: "Appointments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AppointmentId",
                table: "Transactions",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_PatientDetails_PatientDetailsId",
                table: "Appointments",
                column: "PatientDetailsId",
                principalTable: "PatientDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Appointments_AppointmentId",
                table: "Transactions",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_UserDetails_User_PersonalId",
                table: "Transactions",
                column: "User_PersonalId",
                principalTable: "UserDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
