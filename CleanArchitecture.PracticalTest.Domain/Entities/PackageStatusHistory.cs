using CleanArchitecture.PracticalTest.Domain.Common;
using CleanArchitecture.PracticalTest.Domain.Enums;

namespace CleanArchitecture.PracticalTest.Domain.Entities
{
    public class PackageStatusHistory : BaseDomainModel
    {
        public Guid PackageId { get; private set; }
        public PackageStatus Status { get; private set; }
        public DateTimeOffset ChangedAt { get; private set; }
        public string? Reason { get; private set; }

        private PackageStatusHistory() { }

        public PackageStatusHistory(Guid packageId, PackageStatus status, DateTimeOffset changedAt, string? reason)
        {
            PackageId = packageId;
            Status = status;
            ChangedAt = changedAt;
            Reason = string.IsNullOrWhiteSpace(reason) ? null : reason.Trim();
        }
    }
}
