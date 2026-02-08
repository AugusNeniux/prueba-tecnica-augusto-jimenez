using CleanArchitecture.PracticalTest.Domain.Enums;

namespace CleanArchitecture.PracticalTest.API.Controllers.Requests;

public class UpdatePackageStatusRequest
{
    public PackageStatus NewStatus { get; init; }
    public string? Reason { get; init; }
}
