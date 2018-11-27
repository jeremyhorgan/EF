using System;

namespace TestRepository.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IBusinessPartnerRepository BusinessPartners { get; }

        int Complete();
    }
}