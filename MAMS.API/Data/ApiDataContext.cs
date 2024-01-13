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
        public DbSet<SuserDetails> SuserDetails { get; set; }
        public DbSet<DoctorAvailableDetails> DoctorAvailableDetails { get; set; }
        public DbSet<DoctorDetails> DoctorDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Suser>()
             .HasOne(s => s.SuserDetails)
             .WithOne(sd => sd.Suser)
             .HasForeignKey<SuserDetails>(dd => dd.SuserId);

            // One-to-Many relationship between SuserDetails and DoctorAvailableDetails
            modelBuilder.Entity<SuserDetails>()
                .HasMany(s => s.DoctorAvailableDetails)
                .WithOne(sd => sd.SuserDetails)
                .HasForeignKey(dd => dd.SuserDetailsId);

            modelBuilder.Entity<SuserDetails>()
                .HasOne(s => s.DoctorDetails)
                .WithOne(sd => sd.SuserDetails)
                .HasForeignKey<DoctorDetails>(dd => dd.SuserDetailsId);

            // Additional configurations...

            base.OnModelCreating(modelBuilder);
        }

    }
}
