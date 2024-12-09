
namespace BlasII.QualityOfLife;

/// <summary>
/// Settings regarding the status of Qol modules
/// </summary>
public class QolSettings
{
    /// <summary>
    /// Should cutscenes and quotes be skipped
    /// </summary>
    public bool CutsceneSkip { get; set; }

    /// <summary>
    /// Should the typhoon attack be made fps-independent
    /// </summary>
    public bool ConsistentTyphoon { get; set; }

    /// <summary>
    /// Which story/UI events should be skipped
    /// </summary>
    public int SkipStoryLevel { get; set; }
}
