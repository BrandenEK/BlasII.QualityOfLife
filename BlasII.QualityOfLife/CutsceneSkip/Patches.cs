using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppPlaymaker.UI;
using Il2CppTGK.Game.Cutscenes;

namespace BlasII.QualityOfLife.CutsceneSkip;

/// <summary>
/// Level 3 - Skip cutscenes
/// </summary>
[HarmonyPatch(typeof(PlayCutscene), nameof(PlayCutscene.OnEnter))]
class Cutscene_Skip_Patch
{
    public static bool Prefix(PlayCutscene __instance)
    {
        if (!Main.QualityOfLife.CurrentSettings.CutsceneSkip)
            return true;

        // Don't skip Eviterno defeat cutscene
        if (__instance.cutsceneId?.name == "CTS17_id")
            return true;

        ModLog.Warn("Skipping cutscene: " + __instance.cutsceneId?.name);
        __instance.Finish();
        return false;
    }
}
[HarmonyPatch(typeof(ShowQuote), nameof(ShowQuote.OnEnter))]
class Quote_Skip_Patch
{
    public static bool Prefix(ShowQuote __instance)
    {
        if (!Main.QualityOfLife.CurrentSettings.CutsceneSkip)
            return true;

        ModLog.Warn("Skipping quote: " + __instance.Owner.name);
        __instance.Finish();
        return false;
    }
}
