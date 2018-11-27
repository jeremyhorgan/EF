using System.Text;

namespace TestRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddSwuOrigin : DbMigration
    {
        // ReSharper disable once InconsistentNaming
        private static readonly SwuOriginItem[] swuOriginItems =
        {
            new SwuOriginItem("US", "UUS Production", 0, -1),
            new SwuOriginItem("UK", "UUK Production", 0, -1),
            new SwuOriginItem("NE", "UNL Production", 1, -1),
            new SwuOriginItem("GM", "UD Production", 1, -1),
            new SwuOriginItem("FR", "French", 3, -5185306),
            new SwuOriginItem("EU", "European", 3, -5185306),
            new SwuOriginItem("BR", "Brazil", 5, -16181),
            new SwuOriginItem("JA", "Japan", 5, -16181),
            new SwuOriginItem("FS", "Former Soviet Union", 5, -16181),
            new SwuOriginItem("PC", "China", 5, -16181),
            new SwuOriginItem("RS", "Russian", 5, -16181),
            new SwuOriginItem("KA", "Kazakhstan", 5, -16181),
            new SwuOriginItem("X", "Repro", 9, -16181),
            new SwuOriginItem("XX", "Unknown", 9, -65536),
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
            foreach (var swuOriginItem in swuOriginItems)
            {
                var sql = new StringBuilder("INSERT INTO dbo.SWUOrigins (")
                    .Append("Code,")
                    .Append("Rank,")
                    .Append("Description,")
                    .Append("ColourCode,")
                    .Append("ModificationDate")
                    .Append(") VALUES ('")
                    .Append(swuOriginItem.Code).Append("','")
                    .Append(swuOriginItem.Rank).Append("','")
                    .Append(swuOriginItem.Description).Append("',")
                    .Append(swuOriginItem.ColourCode).Append(",'")
                    .Append($"{DateTime.Now:s}")
                    .Append("')");

                Sql(sql.ToString());
            }
        }

        private void DeleteEntities()
        {
            Sql("DELETE FROM dbo.SWUOrigins");
        }


        sealed class SwuOriginItem
        {
            public SwuOriginItem(string code, string description, int rank, int colourCode)
            {
                Code = code;
                Rank = rank;
                Description = description;
                ColourCode = colourCode;
            }

            public string Code { get; }

            public int Rank { get; }

            public string Description { get; }

            public int ColourCode { get; }
        }
    }
}
