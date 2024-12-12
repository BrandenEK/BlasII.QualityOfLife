using HarmonyLib;
using Il2CppTGK.Game.Components.StatsSystem;

namespace BlasII.QualityOfLife.Modules;

internal class DoubleOrbExperience : BaseModule
{
    public override string Name { get; } = "DoubleOrbExperience";
    public override int Order { get; } = 7;
}

/// <summary>
/// Multiply the orbxp added by 2
/// </summary>
[HarmonyPatch(typeof(StatsComponent), nameof(StatsComponent.AddRewardOrbsXP))]
class StatsComponent_AddRewardOrbsXP_Patch
{
    public static void Prefix(ref int value)
    {
        if (!Main.QualityOfLife.CurrentSettings.DoubleOrbExperience)
            return;

        value *= 2;
    }
}
