namespace DynamicPowerGrid.Models
{
    public enum PowerPlantType
    {
        Solar,
        Wind,
        Coal,
        Gas,
        Hydro,
        Nuclear,
        Geothermal
    }

    public static class PowerPlantTypeExtensions
    {
        public static bool IsMustRunRenewable(this PowerPlantType plantType)
        {
            return plantType == PowerPlantType.Solar || plantType == PowerPlantType.Wind;
        }

        public static bool IsDispatchable(this PowerPlantType plantType)
        {
            return !plantType.IsMustRunRenewable();
        }
    }
}
