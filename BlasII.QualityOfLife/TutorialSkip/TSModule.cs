using UnityEngine;

namespace BlasII.QualityOfLife.TutorialSkip;

internal class TSModule : BaseModule
{
    public override string Name { get; } = "TutorialSkip";
    public override int Order { get; } = 2;
    public override KeyCode DefaultKey { get; } = KeyCode.Keypad2;
}
