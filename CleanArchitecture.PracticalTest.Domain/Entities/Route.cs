using CleanArchitecture.PracticalTest.Domain.Common;
using CleanArchitecture.PracticalTest.Domain.Exceptions;

namespace CleanArchitecture.PracticalTest.Domain.Entities
{
    public class Route : BaseDomainModel
    {
        public string Origin { get; private set; } = null!;
        public string Destination { get; private set; } = null!;
        public decimal DistanceKm { get; private set; }
        public decimal EstimatedTimeHours { get; private set; }

        private Route() { }

        public Route(string origin, string destination, decimal distanceKm, decimal estimatedTimeHours)
        {
            Validate(origin, destination, distanceKm, estimatedTimeHours);

            Origin = origin.Trim();
            Destination = destination.Trim();
            DistanceKm = distanceKm;
            EstimatedTimeHours = estimatedTimeHours;
        }

        private static void Validate(string origin, string destination, decimal distanceKm, decimal estimatedTimeHours)
        {
            if (string.IsNullOrWhiteSpace(origin))
                throw new DomainException("El origen es requerido", "Route.OriginRequired");

            if (string.IsNullOrWhiteSpace(destination))
                throw new DomainException("El destino es requerido", "Route.DestinationRequired");

            if (distanceKm <= 0)
                throw new DomainException("La distancia debe ser mayor a 0", "Route.InvalidDistance");

            if (estimatedTimeHours <= 0)
                throw new DomainException("El tiempo estimado debe ser mayor a 0", "Route.InvalidEstimatedTime");
        }
    }
}
