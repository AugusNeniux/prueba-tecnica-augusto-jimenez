using CleanArchitecture.PracticalTest.Domain.Entities;

namespace CleanArchitecture.PracticalTest.Application.Abstractions.Persistence;

public interface IContextDb
{
    Task AddPackageAsync(Package package, CancellationToken ct);
    Task<Package?> GetPackageByIdAsync(Guid id, CancellationToken ct);
    Task<Package?> GetPackageByIdForUpdateAsync(Guid id, CancellationToken ct);
    Task AddRouteAsync(Route route, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
