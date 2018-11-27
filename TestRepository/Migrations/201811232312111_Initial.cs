namespace TestRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BusinessPartners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        ModifiedBy = c.String(),
                        ModificationDate = c.DateTime(nullable: false),
                        ShortName = c.String(),
                        Name = c.String(),
                        SAPId = c.Long(nullable: false),
                        ColourCodeArgb = c.Int(nullable: false),
                        RegionalGrouping = c.String(),
                        IsCustomer = c.Boolean(nullable: false),
                        IsFabricator = c.Boolean(nullable: false),
                        IsDeconverter = c.Boolean(nullable: false),
                        IsConverter = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contracts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        ModifiedBy = c.String(),
                        ModificationDate = c.DateTime(nullable: false),
                        BusinessPartnerId = c.Int(nullable: false),
                        ContractNumber = c.String(),
                        WeightTolerance = c.Double(nullable: false),
                        ProductContainerSupply = c.String(),
                        FinalNotificationPeriod = c.Int(nullable: false),
                        ProvisionalNotificationPeriod = c.Int(nullable: false),
                        ContainerDeliveryPeriod = c.Int(nullable: false),
                        MinimumAssayTolerance = c.Double(nullable: false),
                        AssayTolerance = c.Double(nullable: false),
                        CollectionDateOffset = c.Int(nullable: false),
                        TransportAllowancePeriod = c.Int(nullable: false),
                        ArbitrationPeriod = c.Int(nullable: false),
                        DeliveryContainerOwnership = c.String(),
                        UrencoBusinessEntityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BusinessPartners", t => t.BusinessPartnerId, cascadeDelete: true)
                .ForeignKey("dbo.UrencoBusinessEntities", t => t.UrencoBusinessEntityId)
                .Index(t => t.BusinessPartnerId)
                .Index(t => t.UrencoBusinessEntityId);
            
            CreateTable(
                "dbo.ContractCylinderOwnershipPermitteds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        ModifiedBy = c.String(),
                        ModificationDate = c.DateTime(nullable: false),
                        ContractId = c.Int(nullable: false),
                        BusinessPartnerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BusinessPartners", t => t.BusinessPartnerId)
                .ForeignKey("dbo.Contracts", t => t.ContractId, cascadeDelete: true)
                .Index(t => t.ContractId)
                .Index(t => t.BusinessPartnerId);
            
            CreateTable(
                "dbo.ContractSWUOriginPermitteds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        ModifiedBy = c.String(),
                        ModificationDate = c.DateTime(nullable: false),
                        ContractId = c.Int(nullable: false),
                        SwuOriginId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contracts", t => t.ContractId, cascadeDelete: true)
                .ForeignKey("dbo.SWUOrigins", t => t.SwuOriginId)
                .Index(t => t.ContractId)
                .Index(t => t.SwuOriginId);
            
            CreateTable(
                "dbo.SWUOrigins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        ModifiedBy = c.String(),
                        ModificationDate = c.DateTime(nullable: false),
                        Code = c.String(),
                        Description = c.String(),
                        Rank = c.Int(nullable: false),
                        ColourCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UrencoBusinessEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        ModifiedBy = c.String(),
                        ModificationDate = c.DateTime(nullable: false),
                        BusinessEntityName = c.String(),
                        UranicsPlantName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contracts", "BusinessEntityId", "dbo.UrencoBusinessEntities");
            DropForeignKey("dbo.ContractSWUOriginPermitteds", "SwuOriginId", "dbo.SWUOrigins");
            DropForeignKey("dbo.ContractSWUOriginPermitteds", "ContractId", "dbo.Contracts");
            DropForeignKey("dbo.ContractCylinderOwnershipPermitteds", "ContractId", "dbo.Contracts");
            DropForeignKey("dbo.ContractCylinderOwnershipPermitteds", "BusinessPartnerId", "dbo.BusinessPartners");
            DropForeignKey("dbo.Contracts", "BusinessPartnerId", "dbo.BusinessPartners");
            DropIndex("dbo.ContractSWUOriginPermitteds", new[] { "SwuOriginId" });
            DropIndex("dbo.ContractSWUOriginPermitteds", new[] { "ContractId" });
            DropIndex("dbo.ContractCylinderOwnershipPermitteds", new[] { "BusinessPartnerId" });
            DropIndex("dbo.ContractCylinderOwnershipPermitteds", new[] { "ContractId" });
            DropIndex("dbo.Contracts", new[] { "BusinessEntityId" });
            DropIndex("dbo.Contracts", new[] { "BusinessPartnerId" });
            DropTable("dbo.UrencoBusinessEntities");
            DropTable("dbo.SWUOrigins");
            DropTable("dbo.ContractSWUOriginPermitteds");
            DropTable("dbo.ContractCylinderOwnershipPermitteds");
            DropTable("dbo.Contracts");
            DropTable("dbo.BusinessPartners");
        }
    }
}
