namespace DynamicPowerGrid.Models
{
    public class PowerPlant
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public PowerPlantType PlantType { get; set; }

        /// <summary>
        /// Maximum rated nameplate capacity in MegaWatts (MW).
        /// </summary>
        public double MaxCapacity { get; set; }

        /// <summary>
        /// Currently available generation capacity based on environmental conditions
        /// (e.g. sun intensity for Solar, water flow for Hydro, or rated max capacity for thermal).
        /// </summary>
        public double AvailableCapacity { get; set; }

        /// <summary>
        /// Current actual production output dispatched into the power grid in MW.
        /// </summary>
        public double CurrentOutput { get; set; }

        /// <summary>
        /// Production factor / Throttle ratio between 0.0 (0%) and 1.0 (100%).
        /// </summary>
        public double ThrottleFactor { get; set; } = 1.0;

        public PowerPlant() { }

        public PowerPlant(string id, string name, PowerPlantType plantType, double maxCapacity, double? availableCapacity = null)
        {
            Id = id;
            Name = name;
            PlantType = plantType;
            MaxCapacity = maxCapacity;
            AvailableCapacity = availableCapacity ?? maxCapacity;
            CurrentOutput = AvailableCapacity;
            ThrottleFactor = 1.0;
        }
    }

    public class GridStateSnapshot
    {
        public double TotalDemand { get; set; }
        public double TotalMustRunGeneration { get; set; }
        public double NetDispatchableDemand { get; set; }
        public double TotalDispatchableAvailableCapacity { get; set; }
        public double DispatchableThrottleRatio { get; set; }
        public double TotalGridOutput { get; set; }
    }
}
