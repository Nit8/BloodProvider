using BloodProvider.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BloodProvider.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        // Add this DbSet
        public DbSet<BloodRequest> BloodRequests { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure BloodRequest relationship
            builder.Entity<BloodRequest>()
                .HasOne(r => r.Hospital)
                .WithMany()
                .HasForeignKey(r => r.HospitalId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
