using System.Linq;
using TestRepository.Model;

namespace TestRepository.Repository
{
    // ReSharper disable once InconsistentNaming
    public class SWUOriginRepository : Repository<SWUOrigin>, ISWUOriginRepository
    {
        public SWUOriginRepository(AppDbContext context) : base(context)
        {
        }

        public SWUOrigin GetFromCode(string code)
        {
            return Context.SWUOrigins.FirstOrDefault(e => e.Code == code);
        }
    }
}