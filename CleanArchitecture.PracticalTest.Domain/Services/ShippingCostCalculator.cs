using CleanArchitecture.PracticalTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.PracticalTest.Domain.Services
{
    public static class ShippingCostCalculator
    {
        private const decimal BaseCost = 50m;
        private const decimal ExtraKgCost = 15m;
        private const decimal CostPerKm = 2.5m;

        public static decimal Calculate(Package package, Route route)
        {
            ArgumentNullException.ThrowIfNull(package);
            ArgumentNullException.ThrowIfNull(route);

            decimal peso = package.WeightKg;
            decimal distancia = route.DistanceKm;

            decimal costoPeso = Math.Max(0m, peso - 1m) * ExtraKgCost;
            decimal costoDistancia = distancia * CostPerKm;

            decimal subtotal = BaseCost + costoPeso + costoDistancia;

            decimal total = package.VolumeCm3 > 500_000m ? subtotal * 1.20m : subtotal;

            return Math.Round(total, 2, MidpointRounding.AwayFromZero);
        }
    }
}
