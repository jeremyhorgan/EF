using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TestDeleteDisconnectedEntityCore
{
    internal class Program
    {
        private static void Main()
        {
            // Fetch a contract
            var contract = DisplayContracts();

            // Re-attach a different contract and delete
            SaveDisconnectedContract(contract.Clone());

            // Display Remaining contracts
            DisplayContracts();
        }

        private static Contract DisplayContracts()
        {
            using (var context = new TestDbContext())
            {
                context.Database.EnsureCreated();

                var businessPartner = context.BusinessPartners
                    .Where(e => e.Name == "Business Partner 1")
                    .Include(bp => bp.Contracts)
                    .ThenInclude(c => c.BusinessEntity)
                    .FirstOrDefault();

                Debug.Assert(businessPartner != null, nameof(businessPartner) + " != null");

                Console.WriteLine(businessPartner.Name);
                foreach (var c in businessPartner.Contracts)
                {
                    Console.WriteLine(c.Name);
                }

                return businessPartner.Contracts.First();
            }
        }

        private static void SaveDisconnectedContract(Contract contract)
        {
            // contract.Id = 0;
            contract.Name = "New Contract " + new Random().Next(1);
            contract.BusinessPartner = null;

            // contract.BusinessPartner.Contracts.Add(contract);
            // contract = new Contract {Name = "Test", BusinessEntityId = contract.BusinessEntityId, BusinessPartnerId = contract.BusinessPartnerId};
            using (var context = new TestDbContext())
            {
                context.ChangeTracker.TrackGraph(contract,
                    e =>
                    {
                        if (e.Entry.IsKeySet)
                        {
                            e.Entry.State = EntityState.Modified;
                        }
                        else
                        {
                            e.Entry.State = EntityState.Unchanged;
                        }
                    });

                context.SaveChanges();
            }
        }

        internal class TestDbContext : DbContext
        {
            private static readonly string ConnectionString = @"Server=.\SQLEXPRESS;Database=TestDeleteDisconnectedEntityCore;Trusted_Connection=True";

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
                optionsBuilder.EnableSensitiveDataLogging();
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                var businessEntities = new List<BusinessEntity>
                {
                    new BusinessEntity {Id = 1, Name = "UEC Ltd"},
                    new BusinessEntity {Id = 2, Name = "LES"}
                };

                var businessPartners = new List<BusinessPartner>
                {
                    new BusinessPartner {Id = 1, Name = "Business Partner 1"},
                    new BusinessPartner {Id = 2, Name = "Business Partner 2"},
                    new BusinessPartner {Id = 3, Name = "Business Partner 3"}
                };

                var contracts = new List<Contract>
                {
                    new Contract {Id = 1, BusinessPartnerId = 1, Name = "Contract 1", BusinessEntityId = 2},
                    new Contract {Id = 2, BusinessPartnerId = 1, Name = "Contract 2", BusinessEntityId = 2},
                    new Contract {Id = 3, BusinessPartnerId = 1, Name = "Contract 3", BusinessEntityId = 2},
                    new Contract {Id = 4, BusinessPartnerId = 2, Name = "Contract 11", BusinessEntityId = 1},
                    new Contract {Id = 5, BusinessPartnerId = 2, Name = "Contract 22", BusinessEntityId = 1},
                };

                modelBuilder.Entity<BusinessEntity>().HasData(businessEntities.ToArray());
                modelBuilder.Entity<BusinessPartner>().HasData(businessPartners.ToArray());
                modelBuilder.Entity<Contract>().HasData(contracts.ToArray());
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

            public Contract Clone()
            {
                var clone = (Contract)MemberwiseClone();
                return clone;
            }
        }

        internal class BusinessEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

    }
}
