using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAMS.API.Migrations
{
    /// <inheritdoc />
    public partial class updateLabResultAndTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Appointments_Appointment_Id",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_Appointment_Id",
                table: "Transactions");

            migrationBuilder.AlterColumn<decimal>(
                name: "Hospital_fee",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Doctor_fee",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "Appointment_Id",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BookingType",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LabResultId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ResultValue",
                table: "LabResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "LabResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNo",
                table: "LabResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "LabResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeSlot",
                table: "LabResults",
                type: "time",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Appointment_Id",
                table: "Transactions",
                column: "Appointment_Id",
                unique: true,
                filter: "[Appointment_Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_LabResultId",
                table: "Transactions",
                column: "LabResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabResults_PatientDetails_PatientId",
                table: "LabResults",
                column: "PatientId",
                principalTable: "PatientDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Appointments_Appointment_Id",
                table: "Transactions",
                column: "Appointment_Id",
                principalTable: "Appointments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_LabResults_LabResultId",
                table: "Transactions",
                column: "LabResultId",
                principalTable: "LabResults",
                principalColumn: "LabResultId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabResults_PatientDetails_PatientId",
                table: "LabResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Appointments_Appointment_Id",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_LabResults_LabResultId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_Appointment_Id",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_LabResultId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BookingType",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "LabResultId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ReferenceNo",
                table: "LabResults");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "LabResults");

            migrationBuilder.DropColumn(
                name: "TimeSlot",
                table: "LabResults");

            migrationBuilder.AlterColumn<decimal>(
                name: "Hospital_fee",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Doctor_fee",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Appointment_Id",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ResultValue",
                table: "LabResults",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "LabResults",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Appointment_Id",
                table: "Transactions",
                column: "Appointment_Id",
                unique: true);

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
