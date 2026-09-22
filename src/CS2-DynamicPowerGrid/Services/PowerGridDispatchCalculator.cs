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

            // 1. Calculate total must-run generation from intermittent renewables (Solar, Wind).
            // Solar and Wind plants output their full available generation determined by weather/time of day.
            // MUST-RUN plants ARE NOT THROTTLED by this calculation, avoiding feedback loop fluctuations.
            double totalMustRunGeneration = 0.0;
            foreach (var plant in plants.Where(p => p.PlantType.IsMustRunRenewable()))
            {
                plant.ThrottleFactor = 1.0;
                plant.CurrentOutput = plant.AvailableCapacity;
                totalMustRunGeneration += plant.CurrentOutput;
            }

            // 2. Compute net electricity demand remaining after renewable must-run generation.
            double netDispatchableDemand = Math.Max(0.0, totalCityDemand - totalMustRunGeneration);

            // 3. Gather all dispatchable power stations (Hydro, Coal, Gas, Nuclear, Geothermal).
            var dispatchablePlants = plants.Where(p => p.PlantType.IsDispatchable()).ToList();
            double totalDispatchableAvailableCapacity = dispatchablePlants.Sum(p => p.AvailableCapacity);

            // 4. Calculate uniform throttle ratio for dispatchable power stations.
            double throttleRatio = 1.0;
            if (totalDispatchableAvailableCapacity > 0)
            {
                throttleRatio = netDispatchableDemand / totalDispatchableAvailableCapacity;
                throttleRatio = Math.Clamp(throttleRatio, 0.0, 1.0);
            }

            // 5. Apply uniform throttle factor across ALL dispatchable plants (including Hydro).
            double totalDispatchableOutput = 0.0;
            foreach (var plant in dispatchablePlants)
            {
                plant.ThrottleFactor = throttleRatio;
                plant.CurrentOutput = plant.AvailableCapacity * throttleRatio;
                totalDispatchableOutput += plant.CurrentOutput;
            }

            double totalGridOutput = totalMustRunGeneration + totalDispatchableOutput;

            return new GridStateSnapshot
            {
                TotalDemand = totalCityDemand,
                TotalMustRunGeneration = totalMustRunGeneration,
                NetDispatchableDemand = netDispatchableDemand,
                TotalDispatchableAvailableCapacity = totalDispatchableAvailableCapacity,
                DispatchableThrottleRatio = throttleRatio,
                TotalGridOutput = totalGridOutput
            };
        }
    }
}
