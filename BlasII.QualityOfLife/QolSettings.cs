using BlasII.ModdingAPI.Config;

namespace BlasII.QualityOfLife
{
    internal class QolSettings
    {
        public readonly bool allowMirabrasGlitches;
        public readonly bool consistentTyphoon;

        public QolSettings(ConfigHandler config)
        {
            allowMirabrasGlitches = config.GetProperty<bool>("Allow_Mirabras_Glitches");
            consistentTyphoon = config.GetProperty<bool>("Consistent_Typhoon");
        }
    }
}
