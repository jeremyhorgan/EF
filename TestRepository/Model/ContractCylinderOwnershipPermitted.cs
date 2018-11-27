using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestRepository.Model
{
    public class ContractCylinderOwnershipPermitted : IEntity
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
        public int ContractId { get; set; }
        [ForeignKey(nameof(ContractId))]
        public Contract Contract { get; set; }
        public int BusinessPartnerId { get; set; }
        [ForeignKey(nameof(BusinessPartnerId))]
        public BusinessPartner BusinessPartner { get; set; }

    }
}