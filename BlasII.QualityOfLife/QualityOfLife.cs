using BlasII.ModdingAPI;
using System.Collections.Generic;

namespace BlasII.QualityOfLife
{
    public class QualityOfLife : BlasIIMod
    {
        private readonly MirabrasGlitches _mirabrasGlitches = new();
        private readonly TyphoonTimer _typhoonTimer = new();

        public QualityOfLife() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

        protected override void OnInitialize()
        {
            ConfigHandler.RegisterDefaultProperties(new Dictionary<string, object>
            {
                { "Allow_Mirabras_Glitches", true },
                { "Consistent_Typhoon", true },
            });
        }

        protected override void OnUpdate()
        {
            if (!LoadStatus.GameSceneLoaded)
                return;

            _mirabrasGlitches.Update();
            _typhoonTimer.Update();
        }

        public static bool IsModuleActive(string setting)
        {
            return Main.QualityOfLife.ConfigHandler.GetProperty<bool>(setting);
        }
    }
}
