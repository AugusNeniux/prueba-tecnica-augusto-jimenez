using CleanArchitecture.PracticalTest.Domain.Common;

namespace CleanArchitecture.PracticalTest.Application.Contracts.Data;

public interface IUnitOfWork : IDisposable
{
    IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseDomainModel;

    Task<int> CompleteAsync();
}