using System;
using NUnit.Framework;
using TestRepository.Model;
using TestRepository.Repository;
// ReSharper disable InconsistentNaming

namespace TestRepository.Tests
{
    [TestFixture]
    public class TestBusinessPartnerRepository
    {
        [Test]
        public void TestGetBusinessPartner()
        {
            // Arrange
            BusinessPartner businessPartner;

            var businessPartnerId = TestInitializeUtils.CreateBusinessPartner("BP-" + Guid.NewGuid());

            // Act
            using (var unitOfWork = new UnitOfWork())
            {
                businessPartner = unitOfWork.BusinessPartners.Get(businessPartnerId);
            }

            // Assert
            Assert.NotNull(businessPartner);
            Assert.AreEqual(0, businessPartner.Contracts.Count);
        }

        [Test]
        public void TestGetBusinessPartnerWithContracts()
        {
            // Arrange
            BusinessPartner businessPartner;
            var businessEntityId = TestInitializeUtils.GetBusinessEntityFromName("LES");
            var businessPartnerId = TestInitializeUtils.CreateBusinessPartner("BP-" + Guid.NewGuid());
            var contractId1 = TestInitializeUtils.CreateContract(businessPartnerId, "C-" + Guid.NewGuid(), businessEntityId);
            var contractId2 = TestInitializeUtils.CreateContract(businessPartnerId, "C-" + Guid.NewGuid(), businessEntityId);

            var swuOriginUKId = TestInitializeUtils.GetSWUOriginFromName("UK");
            var swuOriginUSId = TestInitializeUtils.GetSWUOriginFromName("US");
            TestInitializeUtils.CreateContractSWUOriginPermitted(contractId1, swuOriginUKId);
            TestInitializeUtils.CreateContractSWUOriginPermitted(contractId1, swuOriginUSId);
            TestInitializeUtils.CreateContractSWUOriginPermitted(contractId2, swuOriginUKId);

            TestInitializeUtils.CreateContractCylinderOwnershipPermitteds(contractId1, businessPartnerId);

            // Act
            using (var unitOfWork = new UnitOfWork())
            {
                businessPartner = unitOfWork.BusinessPartners.GetWithContracts(businessPartnerId);
            }

            // Assert - get contracts and related data
            Assert.NotNull(businessPartner);
            Assert.NotNull(businessPartner.Contracts);
            Assert.AreEqual(2, businessPartner.Contracts.Count);
            Assert.AreEqual(2, businessPartner.Contracts[0].SWUOriginsPermitted.Count);
            Assert.AreEqual(1, businessPartner.Contracts[1].SWUOriginsPermitted.Count);
            Assert.AreEqual(1, businessPartner.Contracts[0].CylinderOwnershipsPermitted.Count);
            Assert.NotNull(businessPartner.Contracts[0].CylinderOwnershipsPermitted[0].BusinessPartner);
            Assert.NotNull(businessPartner.Contracts[0].CylinderOwnershipsPermitted[0].Contract);
            Assert.NotNull(businessPartner.Contracts[0].BusinessEntity);
            Assert.NotNull(businessPartner.Contracts[0].SWUOriginsPermitted[0].SwuOrigin);
        }

        [Test]
        public void TestAddBusinessPartner()
        {
            // Arrange
            var businessPartnerName = "BP-" + Guid.NewGuid();
            var businessPartner = new BusinessPartner
            {
                ModifiedBy = "testuser",
                ModificationDate = DateTime.Now,
                Name = businessPartnerName,
                ShortName = businessPartnerName,
                SAPId = 12345
            };

            // Act
            using (var unitOfWork = new UnitOfWork())
            {
                unitOfWork.BusinessPartners.Add(businessPartner);
                unitOfWork.Complete();
            }

            // Assert
            var businessPartnerId = TestInitializeUtils.GetBusinessPartnerFromName(businessPartnerName);
            Assert.AreEqual(businessPartner.Id, businessPartnerId);
        }

        [Test]
        public void TestAddBusinessPartnerWithContracts()
        {
            // Arrange
            BusinessEntity businessEntity;
            SWUOrigin swuOriginUk;

            using (var unitOfWork = new UnitOfWork())
            {
                businessEntity = unitOfWork.BusinessEntities.GetFromName("LES");
                swuOriginUk = unitOfWork.SWUOrigins.GetFromCode("UK");
            }

            var businessPartnerName = "BP-" + Guid.NewGuid();
            var businessPartner = new BusinessPartner
            {
                ModifiedBy = "testuser",
                ModificationDate = DateTime.Now,
                Name = businessPartnerName,
                ShortName = businessPartnerName,
                SAPId = 12345
            };

            var contract1 = new Contract
            {
                BusinessPartner = businessPartner,
                BusinessEntity = businessEntity,
                ModificationDate = DateTime.Now,
                ModifiedBy = "testuser",
                ContractNumber = "C1234"
            };

            var contractCylinderOwnershipPermitted = new ContractCylinderOwnershipPermitted
            {
                ModificationDate = DateTime.Now,
                ModifiedBy = "testuser",
                BusinessPartner = businessPartner,
                Contract = contract1
            };
            contract1.CylinderOwnershipsPermitted.Add(contractCylinderOwnershipPermitted);

            var swuOriginPermitted = new ContractSWUOriginPermitted
            {
                ModificationDate = DateTime.Now,
                ModifiedBy = "testuser",
                Contract = contract1,
                SwuOrigin = swuOriginUk
            };
            contract1.SWUOriginsPermitted.Add(swuOriginPermitted);

            businessPartner.Contracts.Add(contract1);

            // Act
            using (var unitOfWork = new UnitOfWork())
            {
                unitOfWork.BusinessPartners.Add(businessPartner);
                unitOfWork.Complete();
            }

            // Assert
            var swuOriginCount = TestInitializeUtils.GetTableRowCount("SWUOrigins");
            var businessPartnerId = TestInitializeUtils.GetBusinessPartnerFromName(businessPartnerName);
            Assert.AreEqual(businessPartner.Id, businessPartnerId);
            Assert.AreEqual(14, swuOriginCount);
        }

        [Test]
        public void TestUpdateBusinessPartner()
        {
            // Arrange
            BusinessPartner businessPartner;
            var businessPartnerId = TestInitializeUtils.CreateBusinessPartner("BP-" + Guid.NewGuid());

            using (var unitOfWork = new UnitOfWork())
            {
                businessPartner = unitOfWork.BusinessPartners.Get(businessPartnerId);
            }

            // Act
            businessPartner.Name = "Update Name" + Guid.NewGuid();
            businessPartner.IsDirty = true;
            using (var unitOfWork = new UnitOfWork())
            {
                unitOfWork.BusinessPartners.Update(businessPartner);
                unitOfWork.Complete();
            }

            // Assert
            var id = TestInitializeUtils.GetBusinessPartnerFromName(businessPartner.Name);
            Assert.AreEqual(businessPartner.Id, id);
        }

        [Test]
        public void TestUpdateBusinessPartnerWithContracts()
        {
            // Arrange
            var businessEntityId = TestInitializeUtils.GetBusinessEntityFromName("LES");
            var businessPartnerId = TestInitializeUtils.CreateBusinessPartner("BP-" + Guid.NewGuid());
            var contractId1 = TestInitializeUtils.CreateContract(businessPartnerId, "C-" + Guid.NewGuid(), businessEntityId);

            var swuOriginUKId = TestInitializeUtils.GetSWUOriginFromName("UK");
            var swuOriginUSId = TestInitializeUtils.GetSWUOriginFromName("US");
            TestInitializeUtils.CreateContractSWUOriginPermitted(contractId1, swuOriginUKId);
            TestInitializeUtils.CreateContractSWUOriginPermitted(contractId1, swuOriginUSId);

            TestInitializeUtils.CreateContractCylinderOwnershipPermitteds(contractId1, businessPartnerId);

            SWUOrigin swuOriginFR;
            BusinessPartner businessPartner;
            using (var unitOfWork = new UnitOfWork())
            {
                businessPartner = unitOfWork.BusinessPartners.GetWithContracts(businessPartnerId);
                swuOriginFR = unitOfWork.SWUOrigins.GetFromCode("FR");
            }

            // Act        
            // Create a new ContractSWUOriginsPermitted
            var contractSWUOriginPermitted = new ContractSWUOriginPermitted
            {
                Contract = businessPartner.Contracts[0],
                ContractId = businessPartner.Contracts[0].Id,
                ModificationDate = DateTime.Now,
                ModifiedBy = "testuser",
                SwuOrigin = swuOriginFR,
                SwuOriginId = swuOriginFR.Id
            };
            
            businessPartner.Contracts[0].SWUOriginsPermitted.Add(contractSWUOriginPermitted);

            // Change the contract number
            var newContractNumber = "C-" + Guid.NewGuid();
            businessPartner.Contracts[0].ContractNumber = newContractNumber;
            businessPartner.Contracts[0].IsDirty = true;

            using (var unitOfWork = new UnitOfWork())
            {
                unitOfWork.BusinessPartners.Update(businessPartner);
                unitOfWork.Complete();
            }

            // Assert
            var contractId = TestInitializeUtils.GetContractFromName(newContractNumber);
            Assert.AreEqual(businessPartner.Contracts[0].Id, contractId);
            Assert.AreEqual(3, businessPartner.Contracts[0].SWUOriginsPermitted.Count);
        }

        [Test]
        public void TestDeleteBusinessPartner()
        {
            // Arrange
            var businessPartnerName = "BP-" + Guid.NewGuid();
            var businessPartnerId = TestInitializeUtils.CreateBusinessPartner(businessPartnerName);

            // Act
            using (var unitOfWork = new UnitOfWork())
            {
                var businessPartner = unitOfWork.BusinessPartners.Get(businessPartnerId);
                unitOfWork.BusinessPartners.Remove(businessPartner);

                unitOfWork.Complete();
            }

            // Assert
            var id = TestInitializeUtils.GetBusinessEntityFromName(businessPartnerName);
            Assert.AreEqual(0, id);
        }

        [Test]
        public void TestDeleteBusinessPartnerRelatedEntities()
        {
            // Arrange
            var businessPartnerName = "BP-" + Guid.NewGuid();
            var businessEntityId = TestInitializeUtils.GetBusinessEntityFromName("LES");
            var businessPartnerId = TestInitializeUtils.CreateBusinessPartner(businessPartnerName);
            var contractId1 = TestInitializeUtils.CreateContract(businessPartnerId, "C-" + Guid.NewGuid(), businessEntityId);
            var contractId2 = TestInitializeUtils.CreateContract(businessPartnerId, "C-" + Guid.NewGuid(), businessEntityId);

            var swuOriginUKId = TestInitializeUtils.GetSWUOriginFromName("UK");
            var swuOriginUSId = TestInitializeUtils.GetSWUOriginFromName("US");
            TestInitializeUtils.CreateContractSWUOriginPermitted(contractId1, swuOriginUKId);
            TestInitializeUtils.CreateContractSWUOriginPermitted(contractId1, swuOriginUSId);
            TestInitializeUtils.CreateContractSWUOriginPermitted(contractId2, swuOriginUKId);

            TestInitializeUtils.CreateContractCylinderOwnershipPermitteds(contractId1, businessPartnerId);

            // Act
            using (var unitOfWork = new UnitOfWork())
            {
                var businessPartner = unitOfWork.BusinessPartners.Get(businessPartnerId);
                unitOfWork.BusinessPartners.Remove(businessPartner);

                unitOfWork.Complete();
            }

            // Assert
            var id = TestInitializeUtils.GetBusinessEntityFromName(businessPartnerName);
            Assert.AreEqual(0, id);
        }
    }
}
