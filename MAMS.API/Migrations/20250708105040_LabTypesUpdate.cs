using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MAMS.API.Migrations
{
    /// <inheritdoc />
    public partial class LabTypesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LabCategories",
                columns: new[] { "LabCategoryId", "CategoryName", "Description" },
                values: new object[,]
                {
                    { 5, "Kidney Function", "Urea, creatinine, and kidney health" },
                    { 6, "Infectious Diseases", "Dengue, COVID-19, Hepatitis, etc." },
                    { 7, "Hormones & Endocrine", "Thyroid, reproductive hormones" },
                    { 8, "Lipid Profile", "Cholesterol and triglyceride levels" },
                    { 9, "Electrolytes", "Sodium, Potassium, Chloride" },
                    { 10, "Coagulation", "Blood clotting and bleeding tests" },
                    { 11, "Tumor Markers", "Cancer-related markers and screening" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LabCategories",
                keyColumn: "LabCategoryId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "LabCategories",
                keyColumn: "LabCategoryId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "LabCategories",
                keyColumn: "LabCategoryId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "LabCategories",
                keyColumn: "LabCategoryId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "LabCategories",
                keyColumn: "LabCategoryId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "LabCategories",
                keyColumn: "LabCategoryId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "LabCategories",
                keyColumn: "LabCategoryId",
                keyValue: 11);
        }
    }
}
