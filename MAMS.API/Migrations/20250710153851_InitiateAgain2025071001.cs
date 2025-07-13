using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MAMS.API.Migrations
{
    /// <inheritdoc />
    public partial class InitiateAgain2025071001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LabCategories",
                columns: table => new
                {
                    LabCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabCategories", x => x.LabCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Specializations",
                columns: table => new
                {
                    Specializations_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Specializations_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Record_Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specializations", x => x.Specializations_Id);
                });

            migrationBuilder.CreateTable(
                name: "Susers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePictureURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Susers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LabTypes",
                columns: table => new
                {
                    LabTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    LabCategoryId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTypes", x => x.LabTypeId);
                    table.ForeignKey(
                        name: "FK_LabTypes_LabCategories_LabCategoryId",
                        column: x => x.LabCategoryId,
                        principalTable: "LabCategories",
                        principalColumn: "LabCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuserId = table.Column<int>(type: "int", nullable: false),
                    UserTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    First_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Last_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Middle_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birth_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Blood_Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Personal_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalId_Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedicalCouncilRegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Specialization_Id = table.Column<int>(type: "int", nullable: false),
                    Hospital_Affiliation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Doctor_Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Auth_Status = table.Column<int>(type: "int", nullable: false),
                    Created_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created_By = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modified_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified_By = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorDetails_Specializations_Specialization_Id",
                        column: x => x.Specialization_Id,
                        principalTable: "Specializations",
                        principalColumn: "Specializations_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorDetails_Susers_SuserId",
                        column: x => x.SuserId,
                        principalTable: "Susers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorAvailableDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    Available_Day = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    ActiveStatus = table.Column<int>(type: "int", nullable: false),
                    Created_Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorAvailableDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorAvailableDetails_DoctorDetails_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "DoctorDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_PersonalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Doctor_Id = table.Column<int>(type: "int", nullable: false),
                    Availability_Id = table.Column<int>(type: "int", nullable: false),
                    Appointment_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Appoinment_number = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DoctorAvailableDetailsId = table.Column<int>(type: "int", nullable: true),
                    UserDetailsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_DoctorAvailableDetails_DoctorAvailableDetailsId",
                        column: x => x.DoctorAvailableDetailsId,
                        principalTable: "DoctorAvailableDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointments_DoctorDetails_Doctor_Id",
                        column: x => x.Doctor_Id,
                        principalTable: "DoctorDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegisteredUserId = table.Column<int>(type: "int", nullable: true),
                    UserTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PersonalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalIdType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Appointment_Id = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientDetails_Appointments_Appointment_Id",
                        column: x => x.Appointment_Id,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LabResults",
                columns: table => new
                {
                    LabResultId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabTypeId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    BookedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeSlot = table.Column<TimeSpan>(type: "time", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PerformedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResultValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsResultAvailable = table.Column<bool>(type: "bit", nullable: false),
                    BookedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReferenceNo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabResults", x => x.LabResultId);
                    table.ForeignKey(
                        name: "FK_LabResults_LabTypes_LabTypeId",
                        column: x => x.LabTypeId,
                        principalTable: "LabTypes",
                        principalColumn: "LabTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabResults_PatientDetails_PatientId",
                        column: x => x.PatientId,
                        principalTable: "PatientDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Appointment_Id = table.Column<int>(type: "int", nullable: true),
                    Patient_Id = table.Column<int>(type: "int", nullable: false),
                    BookingType = table.Column<int>(type: "int", nullable: false),
                    Doctor_fee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LabResultId = table.Column<int>(type: "int", nullable: true),
                    Hospital_fee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    Created_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created_By = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modified_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified_By = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientDetailsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Appointments_Appointment_Id",
                        column: x => x.Appointment_Id,
                        principalTable: "Appointments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_LabResults_LabResultId",
                        column: x => x.LabResultId,
                        principalTable: "LabResults",
                        principalColumn: "LabResultId");
                    table.ForeignKey(
                        name: "FK_Transactions_PatientDetails_PatientDetailsId",
                        column: x => x.PatientDetailsId,
                        principalTable: "PatientDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuserId = table.Column<int>(type: "int", nullable: false),
                    UserTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    First_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Last_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Middle_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birth_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Blood_Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Personal_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalId_Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created_By = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modified_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified_By = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDetails_Susers_SuserId",
                        column: x => x.SuserId,
                        principalTable: "Susers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDetails_Transactions_TransactionsId",
                        column: x => x.TransactionsId,
                        principalTable: "Transactions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MedicalRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_PersonalId = table.Column<int>(type: "int", nullable: false),
                    Doctor_PersonalId = table.Column<int>(type: "int", nullable: false),
                    Appointment_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Diagnosis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Treatment_plan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DoctorDetailsId = table.Column<int>(type: "int", nullable: true),
                    UserDetailsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalRecords_DoctorDetails_DoctorDetailsId",
                        column: x => x.DoctorDetailsId,
                        principalTable: "DoctorDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalRecords_UserDetails_UserDetailsId",
                        column: x => x.UserDetailsId,
                        principalTable: "UserDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "LabCategories",
                columns: new[] { "LabCategoryId", "CategoryName", "Description" },
                values: new object[,]
                {
                    { 1, "Blood Tests", "Tests related to blood components" },
                    { 2, "Urine Tests", "Urine analysis and infection detection" },
                    { 3, "Diabetes", "Sugar and insulin related tests" },
                    { 4, "Liver Function", "Health of liver and enzymes" },
                    { 5, "Kidney Function", "Urea, creatinine, and kidney health" },
                    { 6, "Infectious Diseases", "Dengue, COVID-19, Hepatitis, etc." },
                    { 7, "Hormones & Endocrine", "Thyroid, reproductive hormones" },
                    { 8, "Lipid Profile", "Cholesterol and triglyceride levels" },
                    { 9, "Electrolytes", "Sodium, Potassium, Chloride" },
                    { 10, "Coagulation", "Blood clotting and bleeding tests" },
                    { 11, "Tumor Markers", "Cancer-related markers and screening" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Doctor_Id",
                table: "Appointments",
                column: "Doctor_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorAvailableDetailsId",
                table: "Appointments",
                column: "DoctorAvailableDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_UserDetailsId",
                table: "Appointments",
                column: "UserDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorAvailableDetails_DoctorId",
                table: "DoctorAvailableDetails",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorDetails_Specialization_Id",
                table: "DoctorDetails",
                column: "Specialization_Id");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorDetails_SuserId",
                table: "DoctorDetails",
                column: "SuserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LabResults_LabTypeId",
                table: "LabResults",
                column: "LabTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LabResults_PatientId_LabTypeId",
                table: "LabResults",
                columns: new[] { "PatientId", "LabTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_LabTypes_LabCategoryId",
                table: "LabTypes",
                column: "LabCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_DoctorDetailsId",
                table: "MedicalRecords",
                column: "DoctorDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_UserDetailsId",
                table: "MedicalRecords",
                column: "UserDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDetails_Appointment_Id",
                table: "PatientDetails",
                column: "Appointment_Id",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PatientDetailsId",
                table: "Transactions",
                column: "PatientDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_SuserId",
                table: "UserDetails",
                column: "SuserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_TransactionsId",
                table: "UserDetails",
                column: "TransactionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_UserDetails_UserDetailsId",
                table: "Appointments",
                column: "UserDetailsId",
                principalTable: "UserDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DoctorAvailableDetails_DoctorAvailableDetailsId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DoctorDetails_Doctor_Id",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_UserDetails_UserDetailsId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "MedicalRecords");

            migrationBuilder.DropTable(
                name: "DoctorAvailableDetails");

            migrationBuilder.DropTable(
                name: "DoctorDetails");

            migrationBuilder.DropTable(
                name: "Specializations");

            migrationBuilder.DropTable(
                name: "UserDetails");

            migrationBuilder.DropTable(
                name: "Susers");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "LabResults");

            migrationBuilder.DropTable(
                name: "LabTypes");

            migrationBuilder.DropTable(
                name: "PatientDetails");

            migrationBuilder.DropTable(
                name: "LabCategories");

            migrationBuilder.DropTable(
                name: "Appointments");
        }
    }
}
