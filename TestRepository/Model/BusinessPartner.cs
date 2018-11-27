using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable InconsistentNaming
namespace TestRepository.Model
{
    public class BusinessPartner : IEntity
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
        public string ShortName { get; set; }
        public string Name { get; set; }
        public long SAPId { get; set; }
        public int ColourCodeArgb { get; set; }
        public String RegionalGrouping { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsFabricator { get; set; }
        public bool IsDeconverter { get; set; }
        public bool IsConverter { get; set; }
        public List<Contract> Contracts { get; } = new List<Contract>();
    }
}
