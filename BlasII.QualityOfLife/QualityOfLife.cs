using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Helpers;

namespace BlasII.QualityOfLife;

/// <summary>
/// Provides customizable options to improve the gameplay experience
/// </summary>
public class QualityOfLife : BlasIIMod
{
    internal QualityOfLife() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    private readonly MirabrasGlitches _mirabrasGlitches = new();
    private readonly TyphoonTimer _typhoonTimer = new();
    // Story skip handled through patches

    /// <inheritdoc cref="QolSettings" />
    public QolSettings CurrentSettings { get; private set; }

    /// <summary>
    /// Initialize handlers
    /// </summary>
    protected override void OnInitialize()
    {
        CurrentSettings = ConfigHandler.Load<QolSettings>();
        MessageHandler.AddGlobalListener(ReceiveSetting);
    }

    /// <summary>
    /// Update modules when in-game
    /// </summary>
    protected override void OnUpdate()
    {
        if (!SceneHelper.GameSceneLoaded)
            return;

        _mirabrasGlitches.Update();
        _typhoonTimer.Update();
    }

    private void ReceiveSetting(string _, string setting, string value)
    {
        switch (setting)
        {
            case "AllowMirabrasGlitches":
            case "amg":
                CurrentSettings.AllowMirabrasGlitches = bool.Parse(value);
                return;
            case "ConsistentTyphoon":
            case "ct":
                CurrentSettings.ConsistentTyphoon = bool.Parse(value);
                return;
            case "SkipStoryLevel":
            case "ssl":
                CurrentSettings.SkipStoryLevel = int.Parse(value);
                return;
        }

        ModLog.Error($"Unknown setting: '{setting}'");
    }
}
