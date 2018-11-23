using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

// ReSharper disable InconsistentNaming
namespace TestUpdateDisconnectedEntity
{
    internal class BusinessPartner
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public long SAPId { get; set; }
        public int ColourCodeArgb { get; set; }
        public String RegionalGrouping { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsFabricator { get; set; }
        public bool IsDeconverter { get; set; }
        public bool IsConverter { get; set; }
        public List<Contract> Contracts { get; set; }
    }

    internal class Contract
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
        public int BusinessPartnerId { get; set; }
        [ForeignKey(nameof(BusinessPartnerId))]
        public BusinessPartner BusinessPartner { get; set; }
        public string ContractNumber { get; set; }
        public double WeightTolerance { get; set; }
        public string ProductContainerSupply { get; set; }
        public int FinalNotificationPeriod { get; set; }
        public int ProvisionalNotificationPeriod { get; set; }
        public int ContainerDeliveryPeriod { get; set; }
        public double MinimumAssayTolerance { get; set; }
        public double AssayTolerance { get; set; }
        public int CollectionDateOffset { get; set; }
        public int TransportAllowancePeriod { get; set; }
        public int ArbitrationPeriod { get; set; }
        public string DeliveryContainerOwnership { get; set; }
        public int UrencoBusinessEntityId { get; set; }
        [ForeignKey(nameof(UrencoBusinessEntityId))]
        public UrencoBusinessEntity UrencoBusinessEntity { get; set; }
        public List<ContractSWUOriginPermitted> SWUOriginsPermitted { get; set; }
        public List<ContractCylinderOwnershipPermitted> CylinderOwnershipsPermitted { get; set; }
    }

    internal class ContractSWUOriginPermitted
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
        public int ContractId { get; set; }
        [ForeignKey(nameof(ContractId))]
        public Contract Contract { get; set; }
        public int SwuOriginId { get; set; }
        [ForeignKey(nameof(SwuOriginId))]
        public SWUOrigin SwuOrigin { get; set; }
    }

    internal class ContractCylinderOwnershipPermitted
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
        public int ContractId { get; set; }
        [ForeignKey(nameof(ContractId))]
        public Contract Contract { get; set; }
        public int BusinessPartnerId { get; set; }
        [ForeignKey(nameof(BusinessPartnerId))]
        public BusinessPartner BusinessPartner { get; set; }

    }

    internal class UrencoBusinessEntity
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
        public string BusinessEntityName { get; set; }
        public string UranicsPlantName { get; set; }
    }

    internal class SWUOrigin
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

    internal class AppDbContext : DbContext
    {
        private static readonly string ConnectionString = @"Data Source=(local)\SQLEXPRESS;Database=DPS2;Trusted_Connection=True";

        public AppDbContext() : base(ConnectionString)
        {
        }

        public DbSet<UrencoBusinessEntity> UrencoBusinessEntities { get; set; }
        public DbSet<SWUOrigin> SWUOrigins { get; set; }
        public DbSet<BusinessPartner> BusinessPartners { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractSWUOriginPermitted> ContractSWUOriginPermitteds { get; set; }
        public DbSet<ContractCylinderOwnershipPermitted> ContractCylinderOwnershipPermitteds { get; set; }
    }
}
