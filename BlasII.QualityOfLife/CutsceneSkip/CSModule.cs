using UnityEngine;

namespace BlasII.QualityOfLife.CutsceneSkip;

internal class CSModule : BaseModule
{
    public override string Name { get; } = "CutsceneSkip";
    public override KeyCode DefaultKey { get; } = KeyCode.Keypad1;
}
