using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable once InconsistentNaming
namespace TestRepository.Model
{
    public class Contract: IEntity
    {
        public int Id { get; set; }
        [NotMapped]
        public bool IsDirty { get; set; }
        [NotMapped]
        public bool IsNew => Id == 0;
        [NotMapped]
        public bool IsDeleted { get; set; }

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
        public int BusinessEntityId { get; set; }
        [ForeignKey(nameof(BusinessEntityId))]
        public BusinessEntity BusinessEntity { get; set; }        
        public List<ContractSWUOriginPermitted> SWUOriginsPermitted { get; } = new List<ContractSWUOriginPermitted>();
        public List<ContractCylinderOwnershipPermitted> CylinderOwnershipsPermitted { get; } = new  List<ContractCylinderOwnershipPermitted>();
    }
}