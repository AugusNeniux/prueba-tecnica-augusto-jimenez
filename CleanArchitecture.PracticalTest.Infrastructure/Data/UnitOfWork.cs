using System.Collections;
using CleanArchitecture.PracticalTest.Application.Contracts.Data;
using CleanArchitecture.PracticalTest.Domain.Common;
using CleanArchitecture.PracticalTest.Infrastructure.Data.Repositories;

namespace CleanArchitecture.PracticalTest.Infrastructure.Data;

public class UnitOfWork(ContextDb dbContext) : IUnitOfWork
{
    private Hashtable? _repositories;
    private readonly ContextDb _dbContext = dbContext;

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseDomainModel
    {
        _repositories ??= [];

        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(BaseRepository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dbContext);

            _repositories.Add(type, repositoryInstance);
        }

        return (IBaseRepository<TEntity>)_repositories[type]!;
    }

    public async Task<int> CompleteAsync()
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            var result = await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return result;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
