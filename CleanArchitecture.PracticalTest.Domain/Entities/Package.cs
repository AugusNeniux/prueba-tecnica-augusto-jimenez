using CleanArchitecture.PracticalTest.Domain.Common;
using CleanArchitecture.PracticalTest.Domain.Enums;
using CleanArchitecture.PracticalTest.Domain.Exceptions;
using CleanArchitecture.PracticalTest.Domain.Services;

namespace CleanArchitecture.PracticalTest.Domain.Entities
{
    public class Package : BaseDomainModel
    {
        public decimal WeightKg { get; private set; }
        public decimal LengthCm { get; private set; }
        public decimal WidthCm { get; private set; }
        public decimal HeightCm { get; private set; }

        public PackageStatus Status { get; private set; }

        private readonly List<PackageStatusHistory> _statusHistory = new();
        public IReadOnlyCollection<PackageStatusHistory> StatusHistory => _statusHistory.AsReadOnly();

        public Guid? RouteId { get; private set; }
        public Route? Route { get; private set; }
        public decimal? ShippingCost { get; private set; }

        private Package() { }

        public decimal VolumeCm3 => LengthCm * WidthCm * HeightCm;

        public bool HasHighVolumeSurcharge => VolumeCm3 > 500_000m;

        public Package(decimal weightKg, decimal lengthCm, decimal widthCm, decimal heightCm)
        {
            ValidateWeight(weightKg);
            ValidateDimensions(lengthCm, widthCm, heightCm);

            WeightKg = weightKg;
            LengthCm = lengthCm;
            WidthCm = widthCm;
            HeightCm = heightCm;

            Status = PackageStatus.Registrado;

            _statusHistory.Add(new PackageStatusHistory(
                packageId: Id,
                status: Status,
                changedAt: DateTimeOffset.UtcNow,
                reason: null));
        }

        private static void ValidateWeight(decimal weightKg)
        {
            if (weightKg < 0.1m || weightKg > 50m)
                throw new DomainException("El peso del paquete debe estar entre 0.1 y 50 kg", "Package.InvalidWeight");
        }

        private static void ValidateDimensions(decimal lengthCm, decimal widthCm, decimal heightCm)
        {
            if (lengthCm > 150 || widthCm > 150 || heightCm > 150)
                throw new DomainException("Ninguna dimensión puede exceder 150 cm", "Package.InvalidDimensions");

            var volume = lengthCm * widthCm * heightCm;

            if (volume > 1_000_000m)
                throw new DomainException("El volumen del paquete excede el máximo permitido", "Package.InvalidVolume");
        }

        public void AssignRoute(Route route)
        {
            ArgumentNullException.ThrowIfNull(route);

            if (Status != PackageStatus.EnBodega)
                throw new DomainException("Solo se puede asignar una ruta cuando el paquete está en EnBodega", "Package.RouteAssignmentOnlyInWarehouse");

            Route = route;
            RouteId = route.Id;

            ShippingCost = ShippingCostCalculator.Calculate(this, route);

            ChangeStatus(PackageStatus.EnTransito, "Ruta asignada");
        }

        public void ChangeStatus(PackageStatus newStatus, string? reason = null)
        {
            if (Status == newStatus)
                return;

            if (IsFinalStatus(Status))
                throw new DomainException("No se puede cambiar el estado de un paquete entregado o devuelto", "Package.StatusLocked");

            if (newStatus == PackageStatus.Devuelto)
            {
                if (Status is PackageStatus.EnBodega or PackageStatus.EnTransito or PackageStatus.EnReparto)
                {
                    Status = newStatus;

                    _statusHistory.Add(new PackageStatusHistory(
                        packageId: Id,
                        status: Status,
                        changedAt: DateTimeOffset.UtcNow,
                        reason: reason));

                    return;
                }

                throw new DomainException("Solo se puede devolver un paquete desde EnBodega, EnTransito o EnReparto", "Package.InvalidReturnTransition");
            }

            var expectedNext = GetNextInSequence(Status);

            if (newStatus != expectedNext)
                throw new DomainException($"Transición inválida. Desde {Status} solo se puede pasar a {expectedNext}" + " (o a Devuelto en los estados permitidos).", "Package.InvalidStatusTransition");

            Status = newStatus;

            _statusHistory.Add(new PackageStatusHistory(
                packageId: Id,
                status: Status,
                changedAt: DateTimeOffset.UtcNow,
                reason: reason));
        }

        private static bool IsFinalStatus(PackageStatus status) => status is PackageStatus.Entregado or PackageStatus.Devuelto;

        private static PackageStatus GetNextInSequence(PackageStatus current)
        {
            return current switch
            {
                PackageStatus.Registrado => PackageStatus.EnBodega,
                PackageStatus.EnBodega => PackageStatus.EnTransito,
                PackageStatus.EnTransito => PackageStatus.EnReparto,
                PackageStatus.EnReparto => PackageStatus.Entregado,
                _ => throw new DomainException($"No existe siguiente estado para {current}", "Package.NoNextStatus")
            };
        }
    }
}
