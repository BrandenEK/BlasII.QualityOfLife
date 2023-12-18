using BlasII.ModdingAPI;
using System.Collections.Generic;

namespace BlasII.QualityOfLife
{
    public class QualityOfLife : BlasIIMod
    {
        private readonly MirabrasGlitches _mirabrasGlitches = new();
        private readonly TyphoonTimer _typhoonTimer = new();
        // Story skip handled through patches

        public QualityOfLife() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

        protected override void OnInitialize()
        {
            ConfigHandler.RegisterDefaultProperties(new Dictionary<string, object>
            {
                { "Allow_Mirabras_Glitches", true },
                { "Consistent_Typhoon", true },
                { "Skip_Story_Level", 1 },
            });
            MessageHandler.AddGlobalListener(ReceiveSetting);
        }

        protected override void OnUpdate()
        {
            if (!LoadStatus.GameSceneLoaded)
                return;

            _mirabrasGlitches.Update();
            _typhoonTimer.Update();
        }

        private void ReceiveSetting(string _, string setting, string value)
        {
            if (int.TryParse(value, out int iValue))
            {
                ConfigHandler.SetProperty(setting, iValue);
                return;
            }

            if (bool.TryParse(value, out bool bValue))
            {
                ConfigHandler.SetProperty(setting, bValue);
                return;
            }

            LogError($"Invalid value type for '{setting}'");
        }

        public static bool IsModuleActive(string setting)
        {
            return Main.QualityOfLife.ConfigHandler.GetProperty<bool>(setting);
        }

        public static bool IsModuleLevelActive(string setting, int level)
        {
            return Main.QualityOfLife.ConfigHandler.GetProperty<int>(setting) >= level;
        }
    }
}
