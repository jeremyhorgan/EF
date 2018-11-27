using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestRepository.Model
{
    public class BusinessEntity : IEntity
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
        public string BusinessEntityName { get; set; }
        public string UranicsPlantName { get; set; }
    }
}