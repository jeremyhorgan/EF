using System.Text;

namespace TestRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUrencoBusinessEntities : DbMigration
    {
        // ReSharper disable once InconsistentNaming
        private static readonly UrencoBusinessEntityItem[] urencoBusinessEntityItems =
        {
            new UrencoBusinessEntityItem("UEC Ltd", "URU"),
            new UrencoBusinessEntityItem("LES", "LES")
        };

        public override void Up()
        {
            AddEntities();
        }

        public override void Down()
        {
            DeleteEntities();
        }

        private void AddEntities()
        {
            foreach (var urencoBusinessEntityItem in urencoBusinessEntityItems)
            {
                // Insert default data into the record
                var sqlStr = new StringBuilder("INSERT INTO dbo.UrencoBusinessEntities (")
                    .Append("BusinessEntityName,")
                    .Append("UranicsPlantName,")
                    .Append("ModificationDate")
                    .Append(") VALUES ('")
                    .Append(urencoBusinessEntityItem.BusinessEntityName).Append("','")
                    .Append(urencoBusinessEntityItem.UranicsPlantName).Append("','")
                    .Append($"{DateTime.Now:s}")
                    .Append("')");

                Sql(sqlStr.ToString());
            }
        }

        private void DeleteEntities()
        {
            Sql("DELETE FROM dbo.UrencoBusinessEntities");
        }
    }

    sealed class UrencoBusinessEntityItem
    {
        public UrencoBusinessEntityItem(string businessEntityName, string uranicsPlantName)
        {
            BusinessEntityName = businessEntityName;
            UranicsPlantName = uranicsPlantName;
        }

        public string BusinessEntityName { get; }

        public string UranicsPlantName { get; }
    }
}
