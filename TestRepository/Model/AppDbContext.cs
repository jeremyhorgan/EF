using System.Data.Entity;
using TestRepository.Migrations;

namespace TestRepository.Model
{
    public class AppDbContext : DbContext
    {
        public static readonly string ConnectionString = @"Data Source=(local)\SQLEXPRESS;Database=DPS0;Trusted_Connection=True";

        public AppDbContext() : base(ConnectionString)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDbContext, AppDbContextConfiguration>());
        }

        public DbSet<BusinessEntity> BusinessEntities { get; set; }
        public DbSet<SWUOrigin> SWUOrigins { get; set; }
        public DbSet<BusinessPartner> BusinessPartners { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractSWUOriginPermitted> ContractSWUOriginPermitteds { get; set; }
        public DbSet<ContractCylinderOwnershipPermitted> ContractCylinderOwnershipPermitteds { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contract>()
                .HasRequired(c => c.BusinessEntity)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ContractSWUOriginPermitted>()
                .HasRequired(c => c.SwuOrigin)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ContractCylinderOwnershipPermitted>()
                .HasRequired(c => c.BusinessPartner)
                .WithMany()
                .WillCascadeOnDelete(false);
        }
    }
}