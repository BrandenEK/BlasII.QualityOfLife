using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppPlaymaker.UI;
using Il2CppSystem;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Cutscenes;
using System.Linq;
using UnityEngine;

namespace BlasII.QualityOfLife.Modules;

internal class CutsceneSkip : BaseModule
{
    public override string Name { get; } = "CutsceneSkip";
    public override int Order { get; } = 1;
}

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

        if (FADE_CUTSCENES.Contains(name))
            FadeWindowLogic_FadeAsync_Patch_CS.FADE_FLAG = true;

        ModLog.Warn("Skipping cutscene: " + name);
        __instance.Finish();
        return false;
    }

    private static readonly string[] BANNED_CUTSCENES = ["CTS17_id"];
    private static readonly string[] FADE_CUTSCENES = ["CTS08_id", "CTS10_id", "CTS12_id"];
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

/// <summary>
/// Force the fadeout to be black after skipping certain cutscenes
/// </summary>
[HarmonyPatch(typeof(FadeWindowLogic), nameof(FadeWindowLogic.FadeAsync), typeof(float), typeof(Action), typeof(Color))]
class FadeWindowLogic_FadeAsync_Patch_CS
{
    public static void Prefix(FadeWindowLogic __instance, ref Color targetColor)
    {
        if (!FADE_FLAG)
            return;

        ModLog.Info("Forcing fade to black");
        targetColor = Color.black;
        FADE_FLAG = false;
    }

    public static bool FADE_FLAG { get; set; }
}
