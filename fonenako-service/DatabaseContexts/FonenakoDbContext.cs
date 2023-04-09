using fonenako.Models;
using fonenako_service.Models;
using Microsoft.EntityFrameworkCore;

namespace fonenako.DatabaseContexts
{

    public class FonenakoDbContext : DbContext
    {
        public DbSet<LeaseOffer> LeaseOffers { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Area> Areas { get; set; }

        public FonenakoDbContext(DbContextOptions<FonenakoDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<City>().HasKey(city => city.CityId);
            modelBuilder.Entity<Area>().HasKey(area => area.AreaId);
            modelBuilder.Entity<City>()
                .HasMany<Area>()
                .WithOne()
                .HasForeignKey(area => area.CityId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LeaseOffer>().HasKey(table => table.LeaseOfferID);
            modelBuilder.Entity<LeaseOffer>()
                .HasOne<Area>()
                .WithMany()
                .HasForeignKey(LeaseOffer => LeaseOffer.AreaId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}