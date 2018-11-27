using System.Data.Entity.Infrastructure;

namespace TestRepository.Model
{
    public class MigrationsContextFactory : IDbContextFactory<AppDbContext>
    {
        public AppDbContext Create()
        {
            return new AppDbContext();
        }
    }
}
