using UnityEngine;

namespace BlasII.QualityOfLife.BossIntroSkip;

internal class BISModule : BaseModule
{
    public override string Name { get; } = "BossIntroSkip";
    public override int Order { get; } = 3;
    public override KeyCode DefaultKey { get; } = KeyCode.Keypad3;
}
