using BlasII.ModdingAPI;

namespace BlasII.QualityOfLife
{
    public class QualityOfLife : BlasIIMod
    {
        private readonly MirabrasGlitches _mirabrasGlitches = new();

        public QualityOfLife() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

        protected override void OnUpdate()
        {
            if (!LoadStatus.GameSceneLoaded)
                return;

            _mirabrasGlitches.Update();
        }
    }
}
