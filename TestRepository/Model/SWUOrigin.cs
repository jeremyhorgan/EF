using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable once InconsistentNaming
namespace TestRepository.Model
{
    public class SWUOrigin : IEntity
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
        public string Code { get; set; }
        public string Description { get; set; }
        public int Rank { get; set; }
        public int ColourCode { get; set; }
    }
}