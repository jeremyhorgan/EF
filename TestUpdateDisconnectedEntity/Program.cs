using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
// ReSharper disable InconsistentNaming

namespace TestUpdateDisconnectedEntity
{
    internal class Program
    {
        private static void Main()
        {
            var businessPartner = FetchBusinessPartner();
            DisplayBusinessPartner(businessPartner);

            var contracts = FetchContracts(businessPartner);
            var contract = contracts.First();
            DisplayContract(contract);

            AddContractSWOriginPermitted(contract);
            // AddNewContractWithSWOriginPermitted(businessPartner);
        }

        private static void DisplayBusinessPartner(BusinessPartner businessPartner)
        {
            Console.WriteLine($"Business Partner:{businessPartner.Id} {businessPartner.Name}");
        }

        private static void DisplayContract(Contract contract)
        {
            Console.WriteLine($"Business Partner:{contract.BusinessPartner.Id} {contract.BusinessPartner.Name}");
            Console.WriteLine($"Contract No:                {contract.Id} {contract.ContractNumber}");
            Console.WriteLine($"SWUOriginsPermitted:            {contract.SWUOriginsPermitted.Count}");
            Console.WriteLine($"CylinderOwnershipsPermitted:    {contract.CylinderOwnershipsPermitted.Count}");
        }

        private static BusinessPartner FetchBusinessPartner()
        {
            using (var context = new AppDbContext())
            {
                return context.BusinessPartners.FirstOrDefault();
            }
        }

        private static IEnumerable<Contract> FetchContracts(BusinessPartner businessPartner)
        {
            using (var context = new AppDbContext())
            {
                return context.Contracts
                    .Include(x => x.UrencoBusinessEntity)
                    .Include(x => x.BusinessPartner)
                    .Include(x => x.SWUOriginsPermitted)
                    .Include(x => x.SWUOriginsPermitted.Select(s => s.SwuOrigin))
                    .Include(x => x.CylinderOwnershipsPermitted)                    
                    .ToList();
            }
        }

        private static List<SWUOrigin> FetchSWUOrigins()
        {
            using (var context = new AppDbContext())
            {
                return context.SWUOrigins.ToList();
            }
        }

        private static void AddContractSWOriginPermitted(Contract contract)
        {
            var swOrigins = FetchSWUOrigins();
            contract.ContractNumber = "12345";
            var contractSWUOriginPermitted = new ContractSWUOriginPermitted
            {
                // ContractId = contract.Id,
                Contract = contract,
                ModificationDate = DateTime.Now,
                ModifiedBy = "jhorgan",     
                SwuOrigin = swOrigins[1],
                // SwuOriginId = swOrigins[1].Id
            };

            contract.SWUOriginsPermitted.Add(contractSWUOriginPermitted);

            using (var context = new AppDbContext())
            {
                context.Contracts.Attach(contract);
                context.Entry(contract).State = EntityState.Modified;
                context.Entry(contract.BusinessPartner).State = EntityState.Unchanged;
                context.Entry(contract.UrencoBusinessEntity).State = EntityState.Unchanged;

                foreach (var swuOriginsPermitted in contract.SWUOriginsPermitted)
                {
                    var state = swuOriginsPermitted.Id == 0 ? EntityState.Added : EntityState.Unchanged;
                    context.Entry(swuOriginsPermitted).State = state;

                    // context.Entry(contractSWUOriginPermitted.SwuOrigin).State = EntityState.Unchanged;
                }

                context.SaveChanges();
           }
        }

        private static void AddNewContractWithSWOriginPermitted(BusinessPartner businessPartner)
        {
            var swOrigins = FetchSWUOrigins();

            var contract = new Contract
            {
                ContractNumber = "1234567",
                ProductContainerSupply = "1234567",
                DeliveryContainerOwnership = "1234567",
                BusinessPartner = businessPartner,
                BusinessPartnerId = businessPartner.Id,
                UrencoBusinessEntityId = 1,
                ModificationDate = DateTime.Now,
                ModifiedBy = "jhorgan"
            };

            var contractSWUOriginPermitted = new ContractSWUOriginPermitted
            {
                ModificationDate = DateTime.Now,
                ModifiedBy = "jhorgan",
                SwuOrigin = swOrigins[1],
                SwuOriginId = swOrigins[1].Id
            };

            contract.SWUOriginsPermitted = new List<ContractSWUOriginPermitted> {contractSWUOriginPermitted};

            using (var context = new AppDbContext())
            {
                context.Entry(contract).State = EntityState.Added;
                context.Entry(contract.BusinessPartner).State = EntityState.Unchanged;
                
                foreach (var swuOriginsPermitted in contract.SWUOriginsPermitted)
                {
                    var state = swuOriginsPermitted.Id == 0 ? EntityState.Added : EntityState.Unchanged;
                    context.Entry(swuOriginsPermitted).State = state;

                    context.Entry(contractSWUOriginPermitted.SwuOrigin).State = EntityState.Unchanged;
                }

                context.SaveChanges();
            }

        }
    }
}
