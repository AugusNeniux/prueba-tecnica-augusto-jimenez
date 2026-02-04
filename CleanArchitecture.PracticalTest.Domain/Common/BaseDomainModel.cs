namespace CleanArchitecture.PracticalTest.Domain.Common;

public enum EntityStatusFilter
{
    OnlyActive,
    OnlyInactive,
    All
}

public abstract class BaseDomainModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid CreatedBy { get; set; } = Guid.Empty;
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsActive { get; set; } = true;
}