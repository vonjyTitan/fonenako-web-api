using fonenako.Models;
using Microsoft.EntityFrameworkCore;

namespace fonenako.DatabaseContexts
{

    public class FonenakoDbContext : DbContext
    {
        public DbSet<LeaseOffer> LeaseOffers { get; set; }

        public FonenakoDbContext(DbContextOptions<FonenakoDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<LeaseOffer>().HasKey(table => table.LeaseOfferID);
        }
    }
}