using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

// ReSharper disable InconsistentNaming
namespace TestUpdateDisconnectedEntity2Core
{
    internal abstract class Entity
    {
        [NotMapped]
        public bool IsNew { get; set; }
        [NotMapped]
        public bool IsUpdated { get; set; }
        [NotMapped]
        public bool IsDeleted { get; set; }
    }

    internal class SiteScenario : Entity
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
        public string Comment { get; set; }
        public List<SiteScenarioDatum> SiteScenarioDatums { get; set; }
    }

    internal class SiteScenarioDatum : Entity
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
        public int SiteScenarioId { get; set; } // Ref
        public int SiteId { get; set; }
        public DateTime SAPDataExportDate { get; set; }
        public string Comment { get; set; }
        public DateTime CylinderStockSAPExportDate { get; set; }
        public DateTime GoodsIssuedSAPExportDate { get; set; }
        public DateTime MaterialStockSAPExportDate { get; set; }
        public DateTime SalesOrdersSAPExportDate { get; set; }
        public DateTime OrdersConvertedSAPExportDate { get; set; }
        public DateTime OneSBottleLinksSAPExportDate { get; set; }
        public DateTime ProductionOrdersSAPExportDate { get; set; }
        public string DataImportedBy { get; set; }
        public DateTime FeedOrdersSAPExportDate { get; set; }
        public SiteScenario SiteScenario { get; set; }
        public List<SiteEnrichment> SiteEnrichments { get; set; }
        public List<SiteCylinderStockDatum> SiteCylinderStockDatums { get; set; }
    }

    internal class SiteEnrichment : Entity
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
        public int ScenarioDatumId { get; set; } // Ref
        public bool IsProcessOrder { get; set; }
        public double RawImportedFirstFillQuantityKilogramsUF6 { get; set; }
        public double RawImportedFirstFillProductAssay { get; set; }
        public double RawOperationalProductAssay { get; set; }
        public int CylinderOwnerId { get; set; }
        public int CylinderSize { get; set; }
        public int FeedTypeId { get; set; } // Ref
        public string CylinderReference { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long SAPOrderNumber { get; set; }
        public double RawOperationalQuantityKilogramsUF6 { get; set; }
        public int? SalesOrderId { get; set; }
        public long LannerOrderNumber { get; set; }
        public int ProductionUnitId { get; set; } // Ref
        public bool IsBlendingDonor { get; set; }
        public string ProductionVersion { get; set; }
        public SiteScenarioDatum ScenarioDatum { get; set; }
        public ProductionUnit ProductionUnit { get; set; }
        public SWUOrigin FeedType { get; set; }
    }

    internal class SiteCylinderStockDatum : Entity
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
        public int ScenarioDatumId { get; set; }
        public int SiteId { get; set; }
        public string CylinderReference { get; set; }
        public int StockState { get; set; }
        public double RawTargetAssay { get; set; }
        public int CylinderSize { get; set; }
        public int CylinderOwnerId { get; set; }
        public int? SWUOriginId { get; set; }
        public int? SalesOrder_Id { get; set; }
        public DateTime? PressureTestDueDate { get; set; }
        public double RawProductQuantityKilogramsUF6 { get; set; }
        public string Location { get; set; }
        public string StorageBin { get; set; }
        public DateTime? LastFillDate { get; set; }
        public string SafeguardsStatus { get; set; }
        public DateTime? SafeguardsDate { get; set; }
        public string Condition { get; set; }
        public DateTime? ConditionDate { get; set; }
        public bool UStamped { get; set; }
        public string ManufacturersSerialNumber { get; set; }
        public string ManufacturersPartNumber { get; set; }
        public DateTime? EmptiedDate { get; set; }
        public double RawTareWeightKilogramsUF6 { get; set; }
        public DateTime? TareWeighingDate { get; set; }
        public double RawGrossWeightKilogramsUF6 { get; set; }
        public DateTime? GrossWeighingDate { get; set; }
        public string Status { get; set; }
        public string CylinderBlockedOrQuality { get; set; }
        public string FeedGrade { get; set; }
        public double U234Level { get; set; }
        public double U236Level { get; set; }
        public SiteScenarioDatum ScenarioDatum { get; set; }
    }

    internal class SWUOrigin : Entity
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Rank { get; set; }
        public int ColourCode { get; set; }
    }

    internal class ProductionUnit : Entity
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
        public string Name { get; set; }
        public int SiteId { get; set; }
        public string EnrichmentPV { get; set; }
        public string SamplingPV { get; set; }
        public string IntraPlantBlendPV { get; set; }
    }

    internal class ProductionPlant : Entity
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string Name { get; set; }
        public int ShortCode { get; set; }
        public double Capacity30BKilogramsUF6 { get; set; }
        public double Capacity48YKilogramsUF6 { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
        public int DefaultSWUOriginId { get; set; } // Ref
        public int PostSamplingMargin { get; set; }
        public int SafePostProductionMargin { get; set; }
        public double RawMaxProductionProductAssay { get; set; }
        public int CooldownPeriod { get; set; }
        public double DefaultHeelKilogramsUF6 { get; set; }
        public int MinPostProductionMargin { get; set; }
        public int MaxPostProductionMargin { get; set; }
        public int MinPostSamplingMargin { get; set; }
        public int StockSamplingLeadTime { get; set; }
        public int BlendCycleTime { get; set; }
        public int SubSamplingOffset { get; set; }
        public int MinIntraPlantBlendOffset { get; set; }
        public int PressureTestExpiryWarning { get; set; }
        public int FeedOrderMinCover { get; set; }
        public int TailsOrderMinCover { get; set; }
        public int WarningDurationDeviation { get; set; }
        public int MaxFillAge { get; set; }
    }

    internal class AppDbContext : DbContext
    {
        private static readonly string ConnectionString =
            @"Data Source=jhorgan-xps\SQLEXPRESS;Database=DPSRealData;Trusted_Connection=True";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SiteScenario>()
                .HasMany(ssd => ssd.SiteScenarioDatums)
                .WithOne(ssd => ssd.SiteScenario)
                .HasForeignKey(ssd => ssd.SiteScenarioId);

            modelBuilder.Entity<SiteScenarioDatum>()
                .HasMany(se => se.SiteEnrichments)
                .WithOne(se => se.ScenarioDatum)
                .HasForeignKey(ssd => ssd.ScenarioDatumId);
            modelBuilder.Entity<SiteScenarioDatum>()
                .HasMany(scs => scs.SiteCylinderStockDatums)
                .WithOne(scs => scs.ScenarioDatum)
                .HasForeignKey(scs => scs.ScenarioDatumId);

            modelBuilder.Entity<SiteEnrichment>()
                .HasOne(se => se.FeedType)
                .WithMany()
                .HasForeignKey(se => se.FeedTypeId);
            modelBuilder.Entity<SiteEnrichment>()
                .HasOne(pu => pu.ProductionUnit)
                .WithMany()
                .HasForeignKey(pu => pu.ProductionUnitId);
        }

        public DbSet<SiteScenario> SiteScenarios { get; set; }
        public DbSet<SiteScenarioDatum> SiteScenarioDatums { get; set; }
        public DbSet<SiteEnrichment> SiteEnrichments { get; set; }
        public DbSet<SiteCylinderStockDatum> SiteCylinderStockDatums { get; set; }
        public DbSet<SWUOrigin> SWUOrigins { get; set; }
        public DbSet<ProductionUnit> ProductionUnits { get; set; }
        public DbSet<ProductionPlant> ProductionPlants { get; set; }
    }
}
