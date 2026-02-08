using CleanArchitecture.PracticalTest.Domain.Common;
using CleanArchitecture.PracticalTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.PracticalTest.Infrastructure.Data;

public class ContextDb(DbContextOptions<ContextDb> options) : DbContext(options)
{
    public DbSet<Package> Packages => Set<Package>();
    public DbSet<Route> Routes => Set<Route>();
    public DbSet<PackageStatusHistory> PackageStatusHistories => Set<PackageStatusHistory>();

    // Sobreescribir el metodo SaveChangesAsync para que se actualicen las propiedades de auditoria
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseDomainModel>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = !entry.Entity.UpdatedBy.HasValue ?
                        Guid.Empty : entry.Entity.UpdatedBy;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aqui se agregan las configuraciones de las entidades para ser mapeadas a la base de datos
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContextDb).Assembly);
    }
}