
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
    /// Should tutorial prompts be skipped
    /// </summary>
    public bool TutorialSkip { get; set; }

    /// <summary>
    /// Should boss intros be skipped
    /// </summary>
    public bool BossIntroSkip { get; set; }

    /// <summary>
    /// Should the typhoon attack be made fps-independent
    /// </summary>
    public bool ConsistentTyphoon { get; set; }

    /// <summary>
    /// Should the envoy altarpieces be preserved after burning
    /// </summary>
    public bool KeepEnvoyAltarpieces { get; set; }

    /// <summary>
    /// Should remembrance items be turned into figures automatically
    /// </summary>
    public bool AutoConvertMementos { get; set; }

    /// <summary>
    /// Should the Mark of Martyrdom points you receive be doubled
    /// </summary>
    public bool DoubleOrbExperience { get; set; }
}
