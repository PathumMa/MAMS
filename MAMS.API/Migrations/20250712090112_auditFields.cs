using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAMS.API.Migrations
{
    /// <inheritdoc />
    public partial class auditFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "LabTypes");

            migrationBuilder.RenameColumn(
                name: "ModifiedDate",
                table: "LabTypes",
                newName: "Modified_Date");

            migrationBuilder.AddColumn<string>(
                name: "Created_By",
                table: "Specializations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Date",
                table: "Specializations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modified_By",
                table: "Specializations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified_Date",
                table: "Specializations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Created_By",
                table: "MedicalRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Date",
                table: "MedicalRecords",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modified_By",
                table: "MedicalRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified_Date",
                table: "MedicalRecords",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Created_By",
                table: "LabTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Date",
                table: "LabTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modified_By",
                table: "LabTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Created_By",
                table: "LabResults",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Date",
                table: "LabResults",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modified_By",
                table: "LabResults",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified_Date",
                table: "LabResults",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Created_By",
                table: "LabCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Date",
                table: "LabCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modified_By",
                table: "LabCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified_Date",
                table: "LabCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created_Date",
                table: "DoctorAvailableDetails",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Created_By",
                table: "DoctorAvailableDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modified_By",
                table: "DoctorAvailableDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified_Date",
                table: "DoctorAvailableDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Created_By",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_Date",
                table: "Appointments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modified_By",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified_Date",
                table: "Appointments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LabCategories",
                keyColumn: "LabCategoryId",
                keyValue: 1,
                columns: new[] { "Created_By", "Created_Date", "Modified_By", "Modified_Date" },
                values: new object[] { null, new DateTime(2025, 7, 12, 14, 31, 11, 982, DateTimeKind.Local).AddTicks(3101), null, null });

            migrationBuilder.UpdateData(
                table: "LabCategories",
                keyColumn: "LabCategoryId",
                keyValue: 2,
                columns: new[] { "Created_By", "Created_Date", "Modified_By", "Modified_Date" },
                values: new object[] { null, new DateTime(2025, 7, 12, 14, 31, 11, 982, DateTimeKind.Local).AddTicks(3117), null, null });

            migrationBuilder.UpdateData(
                table: "LabCategories",
                keyColumn: "LabCategoryId",
                keyValue: 3,
                columns: new[] { "Created_By", "Created_Date", "Modified_By", "Modified_Date" },
                values: new object[] { null, new DateTime(2025, 7, 12, 14, 31, 11, 982, DateTimeKind.Local).AddTicks(3118), null, null });

            migrationBuilder.UpdateData(
                table: "LabCategories",
                keyColumn: "LabCategoryId",
                keyValue: 4,
                columns: new[] { "Created_By", "Created_Date", "Modified_By", "Modified_Date" },
                values: new object[] { null, new DateTime(2025, 7, 12, 14, 31, 11, 982, DateTimeKind.Local).AddTicks(3119), null, null });

            migrationBuilder.UpdateData(
                table: "LabCategories",
                keyColumn: "LabCategoryId",
                keyValue: 5,
                columns: new[] { "Created_By", "Created_Date", "Modified_By", "Modified_Date" },
                values: new object[] { null, new DateTime(2025, 7, 12, 14, 31, 11, 982, DateTimeKind.Local).AddTicks(3120), null, null });

            migrationBuilder.UpdateData(
                table: "LabCategories",
                keyColumn: "LabCategoryId",
                keyValue: 6,
                columns: new[] { "Created_By", "Created_Date", "Modified_By", "Modified_Date" },
                values: new object[] { null, new DateTime(2025, 7, 12, 14, 31, 11, 982, DateTimeKind.Local).AddTicks(3120), null, null });

            migrationBuilder.UpdateData(
                table: "LabCategories",
                keyColumn: "LabCategoryId",
                keyValue: 7,
                columns: new[] { "Created_By", "Created_Date", "Modified_By", "Modified_Date" },
                values: new object[] { null, new DateTime(2025, 7, 12, 14, 31, 11, 982, DateTimeKind.Local).AddTicks(3121), null, null });

            migrationBuilder.UpdateData(
                table: "LabCategories",
                keyColumn: "LabCategoryId",
                keyValue: 8,
                columns: new[] { "Created_By", "Created_Date", "Modified_By", "Modified_Date" },
                values: new object[] { null, new DateTime(2025, 7, 12, 14, 31, 11, 982, DateTimeKind.Local).AddTicks(3122), null, null });

            migrationBuilder.UpdateData(
                table: "LabCategories",
                keyColumn: "LabCategoryId",
                keyValue: 9,
                columns: new[] { "Created_By", "Created_Date", "Modified_By", "Modified_Date" },
                values: new object[] { null, new DateTime(2025, 7, 12, 14, 31, 11, 982, DateTimeKind.Local).AddTicks(3123), null, null });

            migrationBuilder.UpdateData(
                table: "LabCategories",
                keyColumn: "LabCategoryId",
                keyValue: 10,
                columns: new[] { "Created_By", "Created_Date", "Modified_By", "Modified_Date" },
                values: new object[] { null, new DateTime(2025, 7, 12, 14, 31, 11, 982, DateTimeKind.Local).AddTicks(3123), null, null });

            migrationBuilder.UpdateData(
                table: "LabCategories",
                keyColumn: "LabCategoryId",
                keyValue: 11,
                columns: new[] { "Created_By", "Created_Date", "Modified_By", "Modified_Date" },
                values: new object[] { null, new DateTime(2025, 7, 12, 14, 31, 11, 982, DateTimeKind.Local).AddTicks(3124), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created_By",
                table: "Specializations");

            migrationBuilder.DropColumn(
                name: "Created_Date",
                table: "Specializations");

            migrationBuilder.DropColumn(
                name: "Modified_By",
                table: "Specializations");

            migrationBuilder.DropColumn(
                name: "Modified_Date",
                table: "Specializations");

            migrationBuilder.DropColumn(
                name: "Created_By",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "Created_Date",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "Modified_By",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "Modified_Date",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "Created_By",
                table: "LabTypes");

            migrationBuilder.DropColumn(
                name: "Created_Date",
                table: "LabTypes");

            migrationBuilder.DropColumn(
                name: "Modified_By",
                table: "LabTypes");

            migrationBuilder.DropColumn(
                name: "Created_By",
                table: "LabResults");

            migrationBuilder.DropColumn(
                name: "Created_Date",
                table: "LabResults");

            migrationBuilder.DropColumn(
                name: "Modified_By",
                table: "LabResults");

            migrationBuilder.DropColumn(
                name: "Modified_Date",
                table: "LabResults");

            migrationBuilder.DropColumn(
                name: "Created_By",
                table: "LabCategories");

            migrationBuilder.DropColumn(
                name: "Created_Date",
                table: "LabCategories");

            migrationBuilder.DropColumn(
                name: "Modified_By",
                table: "LabCategories");

            migrationBuilder.DropColumn(
                name: "Modified_Date",
                table: "LabCategories");

            migrationBuilder.DropColumn(
                name: "Created_By",
                table: "DoctorAvailableDetails");

            migrationBuilder.DropColumn(
                name: "Modified_By",
                table: "DoctorAvailableDetails");

            migrationBuilder.DropColumn(
                name: "Modified_Date",
                table: "DoctorAvailableDetails");

            migrationBuilder.DropColumn(
                name: "Created_By",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Created_Date",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Modified_By",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Modified_Date",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "Modified_Date",
                table: "LabTypes",
                newName: "ModifiedDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "MedicalRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "LabTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created_Date",
                table: "DoctorAvailableDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
