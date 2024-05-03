using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAMS.API.Migrations
{
    /// <inheritdoc />
    public partial class newDto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Available_Time",
                table: "DoctorAvailableDetails");

            migrationBuilder.AlterColumn<int>(
                name: "Available_Day",
                table: "DoctorAvailableDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "DoctorAvailableDetails",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "DoctorAvailableDetails",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "DoctorAvailableDetails");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "DoctorAvailableDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Available_Day",
                table: "DoctorAvailableDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Available_Time",
                table: "DoctorAvailableDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
