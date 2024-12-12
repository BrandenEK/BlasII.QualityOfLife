using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppTGK.Game.Components.StatsSystem;

namespace BlasII.QualityOfLife.Modules;

internal class DoubleOrbExperience : BaseModule
{
    public override string Name { get; } = "DoubleOrbExperience";
    public override int Order { get; } = 7;
}

[HarmonyPatch(typeof(StatsComponent), nameof(StatsComponent.AddRewardOrbsXP))]
class StatsComponent_AddRewardOrbsXP_Patch
{
    public static void Prefix(ref int value)
    {
        ModLog.Info($"adding {value} orb xp");

        if (!Main.QualityOfLife.CurrentSettings.DoubleOrbExperience)
            return;

        value *= 2;
        ModLog.Info($"adding {value} orb xp");
    }
}
