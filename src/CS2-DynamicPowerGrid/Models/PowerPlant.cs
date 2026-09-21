namespace DynamicPowerGrid.Models
{
    public class PowerPlant
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public PowerPlantType PlantType { get; set; }
        public double MaxCapacity { get; set; }
        public double CurrentOutput { get; set; }
        public double ThrottleFactor { get; set; } = 1.0;

        public PowerPlant() { }

        public PowerPlant(string id, string name, PowerPlantType plantType, double maxCapacity)
        {
            Id = id;
            Name = name;
            PlantType = plantType;
            MaxCapacity = maxCapacity;
            CurrentOutput = maxCapacity;
            ThrottleFactor = 1.0;
        }
    }

    public class GridStateSnapshot
    {
        public double TotalDemand { get; set; }
        public double TotalMustRunGeneration { get; set; }
        public double NetDispatchableDemand { get; set; }
        public double TotalDispatchableMaxCapacity { get; set; }
        public double DispatchableThrottleRatio { get; set; }
        public double TotalGridOutput { get; set; }
    }
}
