using DynamicPowerGrid.Systems;

namespace DynamicPowerGrid
{
    /// <summary>
    /// Entry point for Cities: Skylines II Mod.
    /// When game assemblies (Game.dll, Colossal.UI, Unity.Entities) are available in the game installation directory,
    /// this class implements Game.Modding.IMod to register DynamicPowerGridSystem into the GameSystemBase simulation update loop.
    /// </summary>
    public class Mod
    {
        public static string ModId => "CS2-DynamicPowerGrid";
        public static string ModName => "Dynamic Power Grid Manager";
        public static string ModVersion => "1.0.0";

        public DynamicPowerGridSystem? GridSystem { get; private set; }

        public void OnLoad()
        {
            GridSystem = new DynamicPowerGridSystem();
        }

        public void OnUnload()
        {
            GridSystem = null;
        }
    }
}
