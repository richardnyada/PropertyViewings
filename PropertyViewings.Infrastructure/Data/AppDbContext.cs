using Microsoft.EntityFrameworkCore;
using PropertyViewings.Domain.Entities;

namespace PropertyViewings.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ViewingBooking> Viewings => Set<ViewingBooking>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViewingBooking>()
                .HasIndex(x => new { x.PropertyId, x.StartTimeUtc })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
