using TestRepository.Model;

namespace TestRepository.Repository
{
    // ReSharper disable once InconsistentNaming
    public interface ISWUOriginRepository : IRepository<SWUOrigin>
    {
        SWUOrigin GetFromCode(string code);
    }
}