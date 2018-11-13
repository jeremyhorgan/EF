using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Linq;

namespace TestDeleteDisconnectedEntity
{
    internal class Program
    {
        private static void Main()
        {
            Contract contract;

            // Fetch a contract and detach
            using (var context = new TestDbContext())
            {
                var businessPartner = context.BusinessPartners
                    .Include(bp => bp.Contracts)
                    .Include(bp => bp.Contracts.Select(be => be.BusinessEntity))
                    .FirstOrDefault(e => e.Name == "Business Partner 1");

                Debug.Assert(businessPartner != null, nameof(businessPartner) + " != null");

                Console.WriteLine(businessPartner.Name);
                foreach (var c in businessPartner.Contracts)
                {
                    Console.WriteLine(c.Name);
                }

                contract = businessPartner.Contracts.First();
            }

            // Re-attach the contract and delete
            using (var context = new TestDbContext())
            {
                Console.WriteLine($"Deleting contract: {contract.Name}");

                context.Entry(contract).State = EntityState.Deleted;

                context.SaveChanges();
            }

            // Remaining contracts
            using (var context = new TestDbContext())
            {
                var businessPartner = context.BusinessPartners
                    .Include(bp => bp.Contracts)
                    .Include(bp => bp.Contracts.Select(be => be.BusinessEntity))
                    .FirstOrDefault(e => e.Name == "Business Partner 1");

                Debug.Assert(businessPartner != null, nameof(businessPartner) + " != null");

                Console.WriteLine(businessPartner.Name);
                foreach (var c in businessPartner.Contracts)
                {
                    Console.WriteLine(c.Name);
                }
            }
        }

        internal class TestDbContext : DbContext
        {
            private static readonly string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=c:\Work\EF\TestDeleteDisconnectedEntity.mdf;Integrated Security=True;Connect Timeout=30";

            public TestDbContext() : base(ConnectionString)
            {
                Database.SetInitializer(new TestDbInitializer());
            }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            }

            public DbSet<BusinessPartner> BusinessPartners { get; set; }
            public DbSet<Contract> Contracts { get; set; }
            public DbSet<BusinessEntity> BusinessEntities { get; set; }
        }

        internal class BusinessPartner
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public List<Contract> Contracts { get; private set; } = new List<Contract>();
        }

        internal class Contract
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public int BusinessPartnerId { get; set; }

            [ForeignKey(nameof(BusinessPartnerId))]
            public BusinessPartner BusinessPartner { get; set; }

            public int BusinessEntityId { get; set; }

            [ForeignKey(nameof(BusinessEntityId))]
            public BusinessEntity BusinessEntity { get; set; }

        }

        internal class BusinessEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        internal class TestDbInitializer : DropCreateDatabaseAlways<TestDbContext>
        {
            protected override void Seed(TestDbContext context)
            {
                var businessEntities = new List<BusinessEntity>
                {
                    new BusinessEntity {Name = "UEC Ltd"},
                    new BusinessEntity {Name = "LES"}
                };

                var businessPartners = new List<BusinessPartner>
                {
                    new BusinessPartner {Name = "Business Partner 1"},
                    new BusinessPartner {Name = "Business Partner 2"},
                    new BusinessPartner {Name = "Business Partner 3"}
                };

                var contracts = new List<Contract>
                {
                    new Contract {BusinessPartner = businessPartners[0], Name = "Contract 1", BusinessEntity = businessEntities[1]},
                    new Contract {BusinessPartner = businessPartners[0], Name = "Contract 2", BusinessEntity = businessEntities[1]},
                    new Contract {BusinessPartner = businessPartners[0], Name = "Contract 3", BusinessEntity = businessEntities[1]},
                    new Contract {BusinessPartner = businessPartners[1], Name = "Contract 11", BusinessEntity = businessEntities[0]},
                    new Contract {BusinessPartner = businessPartners[1], Name = "Contract 22", BusinessEntity = businessEntities[0]},
                };

                context.BusinessEntities.AddRange(businessEntities);
                context.BusinessPartners.AddRange(businessPartners);
                context.Contracts.AddRange(contracts);

                base.Seed(context);
            }
        }

    }

}
