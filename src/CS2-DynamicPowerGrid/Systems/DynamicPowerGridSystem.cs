using System.Collections.Generic;
using DynamicPowerGrid.Models;
using DynamicPowerGrid.Services;

namespace DynamicPowerGrid.Systems
{
    public class DynamicPowerGridSystem
    {
        private readonly PowerGridDispatchCalculator _calculator;
        private readonly List<PowerPlant> _registeredPowerPlants;

        public bool IsEnabled { get; set; } = true;

        public DynamicPowerGridSystem()
        {
            _calculator = new PowerGridDispatchCalculator();
            _registeredPowerPlants = new List<PowerPlant>();
        }

        public void RegisterPowerPlant(PowerPlant plant)
        {
            if (plant != null && !_registeredPowerPlants.Exists(p => p.Id == plant.Id))
            {
                _registeredPowerPlants.Add(plant);
            }
        }

        public void UnregisterPowerPlant(string plantId)
        {
            _registeredPowerPlants.RemoveAll(p => p.Id == plantId);
        }

        public IReadOnlyList<PowerPlant> GetPowerPlants() => _registeredPowerPlants.AsReadOnly();

        public GridStateSnapshot OnUpdate(double currentCityDemand)
        {
            if (!IsEnabled)
            {
                return new GridStateSnapshot();
            }

            return _calculator.DispatchPower(_registeredPowerPlants, currentCityDemand);
        }
    }
}
