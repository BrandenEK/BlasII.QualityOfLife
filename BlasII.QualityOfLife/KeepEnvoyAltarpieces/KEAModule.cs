using UnityEngine;

namespace BlasII.QualityOfLife.KeepEnvoyAltarpieces;

internal class KEAModule : BaseModule
{
    public override string Name { get; } = "KeepEnvoyAltarpieces";
    public override int Order { get; } = 5;
    public override KeyCode DefaultKey { get; } = KeyCode.Keypad5;
}
