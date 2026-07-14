using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppPlaymaker.UI;
using Il2CppSystem;
using Il2CppTGK.Game.Cutscenes;
using System.Linq;

namespace BlasII.QualityOfLife.Modules;

internal class CutsceneSkip_1 : BaseModule { }

/// <summary>
/// Skip cutscenes from playmaker
/// </summary>
[HarmonyPatch(typeof(PlayCutscene), nameof(PlayCutscene.PlayAndWait))]
class PlayCutscene_PlayAndWait_Patch_CS
{
    public static bool Prefix(PlayCutscene __instance)
    {
        if (!Main.QualityOfLife.CurrentSettings.CutsceneSkip)
            return true;

        string name = __instance.cutsceneId?.name ?? "Invalid";

        if (BANNED_CUTSCENES.Contains(name))
            return true;

        ModLog.Warn("Skipping cutscene: " + name);
        __instance.Finish();
        return false;
    }

    private static readonly string[] BANNED_CUTSCENES = ["CTS17_id", "CTS08_id", "CTS10_id", "CTS12_id"];
}

/// <summary>
/// Skip quotes from playmaker
/// </summary>
[HarmonyPatch(typeof(ShowQuote), nameof(ShowQuote.OnEnter))]
class ShowQuote_OnEnter_Patch_CS
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
