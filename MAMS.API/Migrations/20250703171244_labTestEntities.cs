using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MAMS.API.Migrations
{
    /// <inheritdoc />
    public partial class labTestEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LabTestCategories",
                columns: table => new
                {
                    LabTestCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTestCategories", x => x.LabTestCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "LabTests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LabTestLabTestCategories",
                columns: table => new
                {
                    LabTestId = table.Column<int>(type: "int", nullable: false),
                    LabTestCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTestLabTestCategories", x => new { x.LabTestId, x.LabTestCategoryId });
                    table.ForeignKey(
                        name: "FK_LabTestLabTestCategories_LabTestCategories_LabTestCategoryId",
                        column: x => x.LabTestCategoryId,
                        principalTable: "LabTestCategories",
                        principalColumn: "LabTestCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabTestLabTestCategories_LabTests_LabTestId",
                        column: x => x.LabTestId,
                        principalTable: "LabTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LabTestResults",
                columns: table => new
                {
                    LabTestResultId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabTestId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    ResultValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResultDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTestResults", x => x.LabTestResultId);
                    table.ForeignKey(
                        name: "FK_LabTestResults_LabTests_LabTestId",
                        column: x => x.LabTestId,
                        principalTable: "LabTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "LabTestCategories",
                columns: new[] { "LabTestCategoryId", "CategoryName", "Description" },
                values: new object[,]
                {
                    { 1, "Blood Tests", "Tests related to blood components" },
                    { 2, "Urine Tests", "Urine analysis and infection detection" },
                    { 3, "Diabetes", "Sugar and insulin related tests" },
                    { 4, "Liver Function", "Health of liver and enzymes" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabTestLabTestCategories_LabTestCategoryId",
                table: "LabTestLabTestCategories",
                column: "LabTestCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LabTestResults_LabTestId",
                table: "LabTestResults",
                column: "LabTestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabTestLabTestCategories");

            migrationBuilder.DropTable(
                name: "LabTestResults");

            migrationBuilder.DropTable(
                name: "LabTestCategories");

            migrationBuilder.DropTable(
                name: "LabTests");
        }
    }
}
