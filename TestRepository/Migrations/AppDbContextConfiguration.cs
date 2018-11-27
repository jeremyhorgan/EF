namespace TestRepository.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class AppDbContextConfiguration : DbMigrationsConfiguration<Model.AppDbContext>
    {
        public AppDbContextConfiguration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Model.AppDbContext context)
        {
        }
    }
}
