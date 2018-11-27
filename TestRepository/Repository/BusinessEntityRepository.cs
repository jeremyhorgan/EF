using System.Linq;
using TestRepository.Model;

namespace TestRepository.Repository
{
    public class BusinessEntityRepository : Repository<BusinessEntity>, IBusinessEntityRepository
    {
        public BusinessEntityRepository(AppDbContext context) : base(context)
        {
        }

        public BusinessEntity GetFromName(string name)
        {
            return Context.BusinessEntities.FirstOrDefault(e => e.BusinessEntityName == name);
        }
    }
}