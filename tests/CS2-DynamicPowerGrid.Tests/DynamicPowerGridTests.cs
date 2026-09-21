using System.Collections.Generic;
using DynamicPowerGrid.Models;
using DynamicPowerGrid.Services;
using DynamicPowerGrid.Systems;
using Xunit;

namespace DynamicPowerGrid.Tests
{
    public class DynamicPowerGridTests
    {
        [Fact]
        public void DispatchPower_ThrottlesDispatchablePlants_WhenSolarPowerRampsUp()
        {
            // Arrange
            var calculator = new PowerGridDispatchCalculator();

            var solarPlant = new PowerPlant("sol-1", "Solar Station", PowerPlantType.Solar, 100)
            {
                ThrottleFactor = 0.0 // Night time, 0 MW solar generation
            };
            var coalPlant = new PowerPlant("coal-1", "Coal Plant", PowerPlantType.Coal, 100);
            var gasPlant = new PowerPlant("gas-1", "Gas Plant", PowerPlantType.Gas, 100);
            var hydroPlant = new PowerPlant("hydro-1", "Hydro Dam", PowerPlantType.Hydro, 100);

            var plants = new List<PowerPlant> { solarPlant, coalPlant, gasPlant, hydroPlant };
            double totalDemand = 150.0; // MW

            // Act 1: Night time (Solar = 0 MW)
            var nightResult = calculator.DispatchPower(plants, totalDemand);

            // Assert Night time: Dispatchable capacity needed = 150 MW out of 300 MW max capacity => 50% throttle
            Assert.Equal(0.0, nightResult.TotalMustRunGeneration);
            Assert.Equal(150.0, nightResult.NetDispatchableDemand);
            Assert.Equal(0.5, nightResult.DispatchableThrottleRatio, precision: 4);
            Assert.Equal(50.0, coalPlant.CurrentOutput);
            Assert.Equal(50.0, gasPlant.CurrentOutput);
            Assert.Equal(50.0, hydroPlant.CurrentOutput);

            // Act 2: Daytime Sunrise (Solar reaches 100% capacity = 100 MW)
            solarPlant.ThrottleFactor = 1.0;
            var dayResult = calculator.DispatchPower(plants, totalDemand);

            // Assert Daytime: Solar generates 100 MW. Remaining demand = 50 MW.
            // Dispatchable throttle ratio = 50 MW / 300 MW max capacity = 1/6 (~0.1667)
            Assert.Equal(100.0, dayResult.TotalMustRunGeneration);
            Assert.Equal(50.0, dayResult.NetDispatchableDemand);
            Assert.Equal(1.0 / 6.0, dayResult.DispatchableThrottleRatio, precision: 4);

            // All dispatchable plants are throttled down uniformly to ~16.67 MW each
            Assert.Equal(100.0 / 6.0, coalPlant.CurrentOutput, precision: 4);
            Assert.Equal(100.0 / 6.0, gasPlant.CurrentOutput, precision: 4);
            Assert.Equal(100.0 / 6.0, hydroPlant.CurrentOutput, precision: 4);

            // Total grid output matches city demand exactly (150 MW)
            Assert.Equal(150.0, dayResult.TotalGridOutput, precision: 4);
        }

        [Fact]
        public void DispatchPower_IncludesNuclearAndGeothermal_InUniformThrottling()
        {
            // Arrange
            var calculator = new PowerGridDispatchCalculator();
            var plants = new List<PowerPlant>
            {
                new PowerPlant("sol-1", "Solar Field", PowerPlantType.Solar, 200) { ThrottleFactor = 1.0 },
                new PowerPlant("nuke-1", "Nuclear Station", PowerPlantType.Nuclear, 200),
                new PowerPlant("geo-1", "Geothermal Plant", PowerPlantType.Geothermal, 100)
            };
            double totalDemand = 300.0; // 300 MW demand

            // Act
            var result = calculator.DispatchPower(plants, totalDemand);

            // Solar covers 200 MW. Net demand for Nuclear + Geothermal = 100 MW.
            // Combined dispatchable max capacity = 300 MW (200 + 100).
            // Throttle ratio = 100 / 300 = 1/3 (~0.3333).
            Assert.Equal(200.0, result.TotalMustRunGeneration);
            Assert.Equal(100.0, result.NetDispatchableDemand);
            Assert.Equal(1.0 / 3.0, result.DispatchableThrottleRatio, precision: 4);
        }

        [Fact]
        public void DynamicPowerGridSystem_RegistersAndUnregistersPlantsCorrectly()
        {
            // Arrange
            var system = new DynamicPowerGridSystem();
            var plant1 = new PowerPlant("coal-1", "Coal Plant 1", PowerPlantType.Coal, 100);
            var plant2 = new PowerPlant("solar-1", "Solar Array", PowerPlantType.Solar, 50);

            // Act
            system.RegisterPowerPlant(plant1);
            system.RegisterPowerPlant(plant2);

            // Assert
            Assert.Equal(2, system.GetPowerPlants().Count);

            // Act: Unregister plant1
            system.UnregisterPowerPlant("coal-1");

            // Assert
            Assert.Single(system.GetPowerPlants());
            Assert.Equal("solar-1", system.GetPowerPlants()[0].Id);
        }
    }
}
