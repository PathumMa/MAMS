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
        public DbSet<LabType> LabTypes { get; set; }
        public DbSet<LabCategory> LabCategories { get; set; }
        public DbSet<LabResult> LabResults { get; set; }

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

            modelBuilder.Entity<LabType>()
                .HasMany(e => e.LabResults)
              .WithOne(r => r.LabType)
              .HasForeignKey(r => r.LabTypeId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LabCategory>()
                .HasMany(e => e.LabTypes)
              .WithOne(t => t.LabCategory)
              .HasForeignKey(t => t.LabCategoryId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LabResult>()
                .HasIndex(e => new { e.PatientId, e.LabTypeId });

            // Dummy seed data for categories
            modelBuilder.Entity<LabCategory>().HasData(
                new LabCategory { LabCategoryId = 1, CategoryName = "Blood Tests", Description = "Tests related to blood components" },
                new LabCategory { LabCategoryId = 2, CategoryName = "Urine Tests", Description = "Urine analysis and infection detection" },
                new LabCategory { LabCategoryId = 3, CategoryName = "Diabetes", Description = "Sugar and insulin related tests" },
                new LabCategory { LabCategoryId = 4, CategoryName = "Liver Function", Description = "Health of liver and enzymes" },
                new LabCategory { LabCategoryId = 5, CategoryName = "Kidney Function", Description = "Urea, creatinine, and kidney health" },
                new LabCategory { LabCategoryId = 6, CategoryName = "Infectious Diseases", Description = "Dengue, COVID-19, Hepatitis, etc." },
                new LabCategory { LabCategoryId = 7, CategoryName = "Hormones & Endocrine", Description = "Thyroid, reproductive hormones" },
                new LabCategory { LabCategoryId = 8, CategoryName = "Lipid Profile", Description = "Cholesterol and triglyceride levels" },
                new LabCategory { LabCategoryId = 9, CategoryName = "Electrolytes", Description = "Sodium, Potassium, Chloride" },
                new LabCategory { LabCategoryId = 10, CategoryName = "Coagulation", Description = "Blood clotting and bleeding tests" },
                new LabCategory { LabCategoryId = 11, CategoryName = "Tumor Markers", Description = "Cancer-related markers and screening" }
            );



            // Additional configurations...

            base.OnModelCreating(modelBuilder);
        }

    }
}
