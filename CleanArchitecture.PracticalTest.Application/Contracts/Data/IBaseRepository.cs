using System.Linq.Expressions;
using CleanArchitecture.PracticalTest.Domain.Common;

namespace CleanArchitecture.PracticalTest.Application.Contracts.Data;

public interface IBaseRepository<T> where T : BaseDomainModel
{
    #region Async Read Methods
    Task<IEnumerable<T>> GetAllAsync(
        List<string>? includes = null, // include clause
        bool disableTracking = true, // tracking clause
        EntityStatusFilter filter = EntityStatusFilter.OnlyActive // filter entity status
    );
    Task<T?> GetByIdAsync(
        Guid id,
        List<string>? includes = null, // include clause
        EntityStatusFilter filter = EntityStatusFilter.OnlyActive // filter entity status
    );
    Task<IEnumerable<T>> GetListAsync(
        Expression<Func<T, bool>> predicate, // where clause
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, // order by clause
        List<string>? includes = null, // include clause
        bool disableTracking = true, // tracking clause
        EntityStatusFilter filter = EntityStatusFilter.OnlyActive // filter entity status
    );
    Task<bool> ExistAsync(
        Expression<Func<T, bool>> predicate,
        List<string>? includes = null, // include clause
        EntityStatusFilter filter = EntityStatusFilter.OnlyActive // filter entity status
    );
    #endregion

    #region Async Write Methods
    Task<T> AddAsync(T entity);
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
    T Update(T entity);
    IEnumerable<T> UpdateRange(IEnumerable<T> entities);
    Task DeleteAsync(Guid id);
    void Delete(T entity);
    Task DeleteRangeAsync(IEnumerable<Guid> ids);
    void DeleteRange(IEnumerable<T> entities);
    void DeleteAll();
    Task SoftDeleteAsync(Guid id);
    void SoftDelete(T entity);
    Task SoftDeleteRangeAsync(IEnumerable<Guid> ids);
    void SoftDeleteRange(IEnumerable<T> entities);
    void SoftDeleteAllAsync();
    #endregion

    #region Specification Methods
    Task<T?> GetWithSpecAsync(ISpecification<T> spec);
    Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec);
    Task<int> CountAsync(ISpecification<T> spec);
    #endregion
}