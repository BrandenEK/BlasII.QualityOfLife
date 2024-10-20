
namespace BlasII.QualityOfLife;

/// <summary>
/// Settings regarding the status of Qol modules
/// </summary>
public class QolSettings
{
    /// <summary>
    /// Should mirabras glitches be allowed
    /// </summary>
    public bool AllowMirabrasGlitches { get; set; } = true;

    /// <summary>
    /// Should the typhoon attack be made fps-independent
    /// </summary>
    public bool ConsistentTyphoon { get; set; } = true;

    /// <summary>
    /// Which story/UI events should be skipped
    /// </summary>
    public int SkipStoryLevel { get; set; } = 1;
}
