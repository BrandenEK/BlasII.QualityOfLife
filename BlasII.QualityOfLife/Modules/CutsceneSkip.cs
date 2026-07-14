using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppPlaymaker.UI;
using Il2CppSystem;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Cutscenes;
using System.Linq;
using UnityEngine;

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

        //if (FADE_CUTSCENES.Contains(name))
        //{
        //    __instance.fadeOut1Color.Value = Color.black;
        //    __instance.fadeOut2Color.Value = Color.black;
        //}
            //FadeWindowLogic_FadeAsync_Patch_CS.FADE_FLAG = true;

        ModLog.Warn("Skipping cutscene: " + name);
        ModLog.Error("Fade: " + __instance.waitForFinalFade.Value);
        //__instance.waitForFinalFade.Value = false;
        __instance.Finish();
        return false;
    }

    private static readonly string[] BANNED_CUTSCENES = ["CTS17_id"];
    private static readonly string[] FADE_CUTSCENES = ["CTS08_id", "CTS10_id", "CTS12_id"];
}

//[HarmonyPatch(typeof(FadeWindowLogic), nameof(FadeWindowLogic.ClearFadeAsync))]
//class y
//{
//    public static void Postfix(FadeWindowLogic __instance)
//    {
//        ModLog.Warn("Closing fade");
//        __instance.colorImage?.color = new Color32(0, 0, 0, 0);
//    }
//}

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
[HarmonyPatch(typeof(FadeWindowLogic), nameof(FadeWindowLogic.FadeToColorAsync))]
class FadeWindowLogic_FadeAsync_Patch_CS
{
    public static void Prefix(FadeWindowLogic __instance, ref Color color)
    {
        //if (!FADE_FLAG)
        //    return;

        //ModLog.Info("Forcing fade to black");
        //color = Color.black;
        //FADE_FLAG = false;
    }

    public static bool FADE_FLAG { get; set; }
}

[HarmonyPatch(typeof(FadeWindowLogic), nameof(FadeWindowLogic.FadeFromCurrentAsync))]
class ty
{
    public static void Prefix(FadeWindowLogic __instance)
    {
        //__instance.colorImage.color = Color.black;
    }
}

/// <summary>
/// Always replace fade current color with black - prevents fade being locked to white after boss defeat
/// </summary>
//[HarmonyPatch(typeof(FadeWindowLogic), nameof(FadeWindowLogic.FadeToCurrentColorAsync))]
//class FadeWindowLogic_FadeToCurrentColorAsync_Patch
//{
//    public static void Prefix(FadeWindowLogic __instance)
//    {
//        __instance.colorImage.color = new Color(0, 0, 0, __instance.colorImage.color.a);
//    }
//}
