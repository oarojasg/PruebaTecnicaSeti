using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace PruebaTecnicaSeti.Models
{
    public class seti_tech_test : DbContext
    {
        public seti_tech_test(DbContextOptions<seti_tech_test> options) : base(options)
        {

        }

        public DbSet<BrokerItem> Broker { get; set; }
        public DbSet<InvestmentProjectItem> InvestmentProject{ get; set; }
        public DbSet<ProjectMovementItem> ProjectMovement { get; set; }
        public DbSet<PeriodItem> Period { get; set; }
        public DbSet<DiscountRateItem> DiscountRate { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PeriodItem>()
                .HasOne(p => p.projectMovement)
                .WithOne(pm => pm.Period)
                .HasForeignKey<ProjectMovementItem>(pm => pm.PeriodId);

            modelBuilder.Entity<DiscountRateItem>()
                .HasOne(dr => dr.PeriodItem)
                .WithOne(pm => pm.discountRate)
                .HasForeignKey<DiscountRateItem>(dr => dr.PeriodId);
        }

        //public DbSet<CatalogBrand> CatalogBrands { get; set; }
        //public DbSet<CatalogType> CatalogTypes { get; set; }
    }
}
