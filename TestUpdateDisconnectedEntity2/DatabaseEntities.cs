using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

// ReSharper disable InconsistentNaming
namespace TestUpdateDisconnectedEntity2
{
    internal class SiteScenario
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
        public string Comment { get; set; }
        public List<SiteScenarioDatum> SiteScenarioDatums { get; set; }
    }

    internal class SiteScenarioDatum
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
        public int SiteScenarioId { get; set; }
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
    }

    internal class SiteEnrichment
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
        public int ScenarioDatumId { get; set; }
        public bool IsProcessOrder { get; set; }
        public double RawImportedFirstFillQuantityKilogramsUF6 { get; set; }
        public double RawImportedFirstFillProductAssay { get; set; }
        public double RawOperationalProductAssay { get; set; }
        public int CylinderOwnerId { get; set; }
        public int CylinderSize { get; set; }
        public int FeedTypeId { get; set; }
        public string CylinderReference { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long SAPOrderNumber { get; set; }
        public double RawOperationalQuantityKilogramsUF6 { get; set; }
        public int? SalesOrderId { get; set; }
        public long LannerOrderNumber { get; set; }
        public int ProductionUnitId { get; set; }
        public bool IsBlendingDonor { get; set; }
        public string ProductionVersion { get; set; }
        public SiteScenarioDatum ScenarioDatum { get; set; }
    }

    internal class AppDbContext : DbContext
    {
        private static readonly string ConnectionString =
            @"Data Source=jhorgan-xps\SQLEXPRESS;Database=DPSRealData;User Id=sa;Password=";

        public AppDbContext() : base(ConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SiteScenario>()
                .HasMany(ssd => ssd.SiteScenarioDatums)
                .WithRequired(ssd => ssd.SiteScenario)
                .HasForeignKey(ssd => ssd.SiteScenarioId);

            modelBuilder.Entity<SiteScenarioDatum>()
                .HasMany(se => se.SiteEnrichments)
                .WithRequired(se => se.ScenarioDatum)
                .HasForeignKey(ssd => ssd.ScenarioDatumId);
        }

        public DbSet<SiteScenario> SiteScenarios { get; set; }
        public DbSet<SiteScenarioDatum> SiteScenarioDatums { get; set; }
        public DbSet<SiteEnrichment> SiteEnrichments { get; set; }
    }
}
