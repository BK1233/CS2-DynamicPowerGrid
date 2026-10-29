using DynamicPowerGrid.Systems;

namespace DynamicPowerGrid
{
    /// <summary>
    /// Entry point for Cities: Skylines II Mod following the standard VS2022 mod template layout.
    /// Supports both game runtime simulation initialization and standalone test/mock execution.
    /// </summary>
    public class Mod
    {
        public static string ModId => "CS2-DynamicPowerGrid";
        public static string ModName => "Dynamic Power Grid Manager";
        public static string ModVersion => "1.0.0";

        public static Setting? Settings { get; private set; }

        public DynamicPowerGridSystem? GridSystem { get; private set; }

        public void OnLoad()
        {
            Settings = new Setting();
            Settings.SetDefaults();

            GridSystem = new DynamicPowerGridSystem();
        }

        public void OnDispose()
        {
            Settings = null;
            GridSystem = null;
        }

        public void OnUnload()
        {
            OnDispose();
        }
    }
}
