namespace DynamicPowerGrid
{
    /// <summary>
    /// Mod settings class for Dynamic Power Grid Manager, matching the standard Cities: Skylines II mod template structure.
    /// Provides options for enabling/disabling automated grid throttling and setting target reserve margins.
    /// </summary>
    public class Setting
    {
        public bool EnableAutomaticThrottling { get; set; } = true;

        public float TargetReserveMarginPercent { get; set; } = 5.0f;

        public void SetDefaults()
        {
            EnableAutomaticThrottling = true;
            TargetReserveMarginPercent = 5.0f;
        }
    }
}
