using MAMS.API.Models;
using MAMS.API.Models.Views;
using Microsoft.EntityFrameworkCore;

namespace MAMS.API.Data
{
    public class ApiDataContext : DbContext
    {
        public ApiDataContext(DbContextOptions<ApiDataContext> options) : base(options)
        {
        }

        public DbSet<Suser> Susers { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<DoctorAvailableDetails> DoctorAvailableDetails { get; set; }
        public DbSet<DoctorDetails> DoctorDetails { get; set; }
        public DbSet<Specializations> Specializations { get; set; }
        public DbSet<MedicalRecords> MedicalRecords { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<PatientDetails> PatientDetails { get; set; }
        public DbSet<Doctors> Doctors { get; set; }
        public DbSet<LabTest> LabTests { get; set; }
        public DbSet<LabTestCategory> LabTestCategories { get; set; }
        public DbSet<LabTestResult> LabTestResults { get; set; }
        public DbSet<LabTestLabTestCategory> LabTestLabTestCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Suser>()
             .HasOne(s => s.UserDetails)
             .WithOne(sd => sd.Suser)
             .HasForeignKey<UserDetails>(dd => dd.SuserId);

            modelBuilder.Entity<Suser>()
             .HasOne(s => s.DoctorDetails)
             .WithOne(sd => sd.Suser)
             .HasForeignKey<DoctorDetails>(dd => dd.SuserId);

            modelBuilder.Entity<DoctorAvailableDetails>()
            .HasOne(d => d.DoctorDetails)
            .WithMany(d => d.AvailableDetails)
            .HasForeignKey(d => d.DoctorId);

            modelBuilder.Entity<Transactions>()
                .HasOne(s => s.Appointments)
                .WithOne(sd => sd.Transactions)
                .HasForeignKey<Transactions>(dd => dd.Appointment_Id);

            modelBuilder.Entity<Appointments>()
                .HasOne(a => a.PatientDetails)
                .WithOne(a => a.Appointments)
                .HasForeignKey<PatientDetails>(dd => dd.Appointment_Id);

            modelBuilder.Entity<Appointments>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.Doctor_Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Doctors>()
                .ToView("View_Doctors").HasNoKey();

            // Configure many-to-many
            modelBuilder.Entity<LabTestLabTestCategory>()
                .HasKey(x => new { x.LabTestId, x.LabTestCategoryId });

            modelBuilder.Entity<LabTestLabTestCategory>()
                .HasOne(x => x.LabTest)
                .WithMany(x => x.LabTestLabTestCategories)
                .HasForeignKey(x => x.LabTestId);

            modelBuilder.Entity<LabTestLabTestCategory>()
                .HasOne(x => x.LabTestCategory)
                .WithMany(x => x.LabTestLabTestCategories)
                .HasForeignKey(x => x.LabTestCategoryId);

            // Dummy seed data for categories
            modelBuilder.Entity<LabTestCategory>().HasData(
                new LabTestCategory { LabTestCategoryId = 1, CategoryName = "Blood Tests", Description = "Tests related to blood components" },
                new LabTestCategory { LabTestCategoryId = 2, CategoryName = "Urine Tests", Description = "Urine analysis and infection detection" },
                new LabTestCategory { LabTestCategoryId = 3, CategoryName = "Diabetes", Description = "Sugar and insulin related tests" },
                new LabTestCategory { LabTestCategoryId = 4, CategoryName = "Liver Function", Description = "Health of liver and enzymes" }
            );



            // Additional configurations...

            base.OnModelCreating(modelBuilder);
        }

    }
}
