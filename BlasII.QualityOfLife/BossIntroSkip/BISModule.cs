using UnityEngine;

namespace BlasII.QualityOfLife.BossIntroSkip;

internal class BISModule : BaseModule
{
    public override string Name { get; } = "BossIntroSkip";
    public override KeyCode DefaultKey { get; } = KeyCode.Keypad3;
}
