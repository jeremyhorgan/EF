using TestRepository.Model;

namespace TestRepository.Repository
{
    public interface IBusinessEntityRepository : IRepository<BusinessEntity>
    {
        BusinessEntity GetFromName(string name);
    }
}