using fonenako.Models;
using fonenako_service.Models;
using Microsoft.EntityFrameworkCore;

namespace fonenako.DatabaseContexts
{

    public class FonenakoDbContext : DbContext
    {
        public DbSet<LeaseOffer> LeaseOffers { get; set; }

        public DbSet<Localisation> Localisations { get; set; }

        public FonenakoDbContext(DbContextOptions<FonenakoDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Localisation>().HasKey(localisation => localisation.LocalisationId);
            modelBuilder.Entity<Localisation>()
                .HasOne(localisation => localisation.Hierarchy)
                .WithMany()
                .HasForeignKey(localisation => localisation.HierarchyId)
                .HasPrincipalKey(hierarchy => hierarchy.LocalisationId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LeaseOffer>().HasKey(table => table.LeaseOfferID);
            modelBuilder.Entity<LeaseOffer>()
                .HasOne<Localisation>()
                .WithMany()
                .HasForeignKey(leaseOffer => leaseOffer.LocalisationId)
                .HasPrincipalKey(a => a.LocalisationId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}