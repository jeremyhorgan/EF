using System.Data.Entity;
using System.Linq;
using NUnit.Framework;
using TestRepository.Model;

namespace TestRepository.Repository
{
    public class BusinessPartnerRepository : Repository<BusinessPartner>, IBusinessPartnerRepository
    {
        public BusinessPartnerRepository(AppDbContext context) : base(context)
        {
        }
        
        public BusinessPartner GetWithContracts(int businessPartnerId)
        {
            return Context.BusinessPartners
                .Where(e => e.Id == businessPartnerId)
                .Include(e => e.Contracts)
                .Include(e => e.Contracts.Select(c => c.BusinessEntity))
                .Include(e => e.Contracts.Select(c => c.SWUOriginsPermitted))
                .Include(e => e.Contracts.Select(c => c.SWUOriginsPermitted.Select(s => s.SwuOrigin)))
                .Include(e => e.Contracts.Select(c => c.CylinderOwnershipsPermitted))
                .FirstOrDefault();
        }

        public BusinessPartner GetWithContracts(BusinessPartner businessPartner)
        {
            Assert.NotNull(businessPartner, nameof(businessPartner) + " parameter must not be null");
            return GetWithContracts(businessPartner.Id);
        }

        public override void Add(BusinessPartner entity)
        {
            // Set all entities in the graph to EntityState.Added
            Context.BusinessPartners.Add(entity);

            // Prevent these entities from being added
            foreach (var contract in entity.Contracts)
            {
                foreach (var swuOriginPermitted in contract.SWUOriginsPermitted)
                {
                    if (swuOriginPermitted.SwuOrigin != null)
                    {
                        Context.Entry(swuOriginPermitted.SwuOrigin).State = EntityState.Unchanged;
                    }
                }
            }
        }

        public override void Update(BusinessPartner entity)
        {
            // Set all entities in the graph to EntityState.Unchanged
            Context.BusinessPartners.Attach(entity);

            // Set entities that may have changed in the graph
            Context.Entry(entity).State = DetermineEntityState(entity);            
            foreach (var contract in entity.Contracts)
            {
                Context.Entry(contract).State = DetermineEntityState(contract);

                foreach (var swuOriginPermitted in contract.SWUOriginsPermitted)
                {
                    Context.Entry(swuOriginPermitted).State = DetermineEntityState(swuOriginPermitted);
                }

                foreach (var cylinderOwnershipsPermitted in contract.CylinderOwnershipsPermitted)
                {
                    Context.Entry(cylinderOwnershipsPermitted).State = DetermineEntityState(cylinderOwnershipsPermitted);
                }
            }
        }

        private static EntityState DetermineEntityState(IEntity entity)
        {
            var state = EntityState.Unchanged;
            if (entity.IsNew)
            {
                state = EntityState.Added;
            }
            else if (entity.IsDirty)
            {
                state = EntityState.Modified;
            }
            else if (entity.IsDeleted && entity.Id != 0)
            {
                state = EntityState.Deleted;
            }

            return state;
        }
    }
}
