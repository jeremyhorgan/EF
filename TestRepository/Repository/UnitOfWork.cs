using System;
using System.Diagnostics;
using TestRepository.Model;

namespace TestRepository.Repository
{
    // ReSharper disable once InconsistentNaming
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Lazy<IBusinessPartnerRepository> _businessPartnerRepository;
        private readonly Lazy<IBusinessEntityRepository> _businessEntityRepository;
        private readonly Lazy<ISWUOriginRepository> _swuOriginRepository;

        public UnitOfWork() : this(new AppDbContext())
        {
        }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _context.Database.Log = Console.Write;

            _businessPartnerRepository = new Lazy<IBusinessPartnerRepository>(() => new BusinessPartnerRepository(_context));
            _businessEntityRepository = new Lazy<IBusinessEntityRepository>(() => new BusinessEntityRepository(_context));
            _swuOriginRepository = new Lazy<ISWUOriginRepository>(() => new SWUOriginRepository(_context));
        }

        public IBusinessPartnerRepository BusinessPartners => _businessPartnerRepository.Value;
        public IBusinessEntityRepository BusinessEntities => _businessEntityRepository.Value;
        public ISWUOriginRepository SWUOrigins => _swuOriginRepository.Value;
        
        public int Complete()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}