using CleanArchitecture.PracticalTest.Application.Abstractions.Persistence;
using CleanArchitecture.PracticalTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.PracticalTest.Infrastructure.Data;

public class ContextDbAdapter : IContextDb
{
    private readonly ContextDb _db;

    public ContextDbAdapter(ContextDb db)
    {
        _db = db;
    }

    public async Task AddPackageAsync(Package package, CancellationToken ct)
    {
        await _db.Set<Package>().AddAsync(package, ct);
    }

    public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);

    public async Task<Package?> GetPackageByIdAsync(Guid id, CancellationToken ct)
    {
        return await _db.Set<Package>()
            .AsNoTracking()
            .Include(x => x.Route)
            .Include(x => x.StatusHistory)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<Package?> GetPackageByIdForUpdateAsync(Guid id, CancellationToken ct)
    {
        return await _db.Set<Package>()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task AddRouteAsync(Route route, CancellationToken ct)
    {
        await _db.Set<Route>().AddAsync(route, ct);
    }

}
