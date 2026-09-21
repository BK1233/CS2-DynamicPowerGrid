using System;
using System.Collections.Generic;
using System.Linq;
using DynamicPowerGrid.Models;

namespace DynamicPowerGrid.Services
{
    public class PowerGridDispatchCalculator
    {
        public GridStateSnapshot DispatchPower(List<PowerPlant> plants, double totalCityDemand)
        {
            if (plants == null) throw new ArgumentNullException(nameof(plants));

            double totalMustRunGeneration = plants
                .Where(p => p.PlantType.IsMustRunRenewable())
                .Sum(p => p.MaxCapacity * p.ThrottleFactor);

            foreach (var plant in plants.Where(p => p.PlantType.IsMustRunRenewable()))
            {
                plant.CurrentOutput = plant.MaxCapacity * plant.ThrottleFactor;
            }

            double netDispatchableDemand = Math.Max(0.0, totalCityDemand - totalMustRunGeneration);
            var dispatchablePlants = plants.Where(p => p.PlantType.IsDispatchable()).ToList();
            double totalDispatchableMaxCapacity = dispatchablePlants.Sum(p => p.MaxCapacity);

            double throttleRatio = 1.0;
            if (totalDispatchableMaxCapacity > 0)
            {
                throttleRatio = netDispatchableDemand / totalDispatchableMaxCapacity;
                throttleRatio = Math.Clamp(throttleRatio, 0.0, 1.0);
            }

            double totalDispatchableOutput = 0.0;
            foreach (var plant in dispatchablePlants)
            {
                plant.ThrottleFactor = throttleRatio;
                plant.CurrentOutput = plant.MaxCapacity * throttleRatio;
                totalDispatchableOutput += plant.CurrentOutput;
            }

            double totalGridOutput = totalMustRunGeneration + totalDispatchableOutput;

            return new GridStateSnapshot
            {
                TotalDemand = totalCityDemand,
                TotalMustRunGeneration = totalMustRunGeneration,
                NetDispatchableDemand = netDispatchableDemand,
                TotalDispatchableMaxCapacity = totalDispatchableMaxCapacity,
                DispatchableThrottleRatio = throttleRatio,
                TotalGridOutput = totalGridOutput
            };
        }
    }
}
