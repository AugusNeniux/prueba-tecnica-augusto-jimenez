namespace CleanArchitecture.PracticalTest.API.Controllers.Requests;

public class AssignRouteRequest
{
    public string Origin { get; init; } = null!;
    public string Destination { get; init; } = null!;
    public decimal DistanceKm { get; init; }
    public decimal EstimatedTimeHours { get; init; }
}
