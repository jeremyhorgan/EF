using TestRepository.Model;

namespace TestRepository.Repository
{
    public interface IBusinessPartnerRepository : IRepository<BusinessPartner>
    {
        BusinessPartner GetWithContracts(int businessPartnerId);
        BusinessPartner GetWithContracts(BusinessPartner businessPartner);
    }
}
