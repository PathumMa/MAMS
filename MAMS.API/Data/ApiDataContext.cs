using MAMS.API.Models;
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
        public DbSet<Appoinments> Appoinments { get; set; }
        public DbSet<Transactions> Transactions { get; set; }

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

            modelBuilder.Entity<DoctorDetails>()
                .HasMany(s => s.DoctorAvailableDetails)
                .WithOne(sd => sd.DoctorDetails)
                .HasForeignKey(dd => dd.DoctorId);

            modelBuilder.Entity<Transactions>()
                .HasOne(s => s.UserDetails)
                .WithOne(sd => sd.Transactions)
                .HasForeignKey<Transactions>(dd => dd.UserDetailsId);

            modelBuilder.Entity<Transactions>()
                .HasOne(s => s.appoinments)
                .WithOne(sd => sd.Transactions)
                .HasForeignKey<Transactions>(dd => dd.AppointmentId);

            // Additional configurations...

            base.OnModelCreating(modelBuilder);
        }

    }
}
