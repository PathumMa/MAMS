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


            modelBuilder.Entity<Doctors>()
                .ToView("View_Doctors").HasNoKey();




            // Additional configurations...

            base.OnModelCreating(modelBuilder);
        }

    }
}
