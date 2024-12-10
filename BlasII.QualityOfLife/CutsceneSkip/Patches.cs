using BlasII.ModdingAPI;
using HarmonyLib;
using Il2Cpp;
using Il2CppPlaymaker.UI;
using Il2CppSystem;
using Il2CppSystem.Threading;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Cutscenes;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Playmaker.Fade;
using System.Linq;
using UnityEngine;
using System.Text;

namespace BlasII.QualityOfLife.CutsceneSkip;

/// <summary>
/// Skip cutscenes from playmaker
/// </summary>
[HarmonyPatch(typeof(PlayCutscene), nameof(PlayCutscene.PlayAndWait))]
class Cutscene_Skip_Patch
{
    public static bool Prefix(PlayCutscene __instance)
    {
        if (!Main.QualityOfLife.CurrentSettings.CutsceneSkip)
            return true;

        // Don't skip Eviterno defeat cutscene
        //if (__instance.cutsceneId?.name == "CTS17_id")
        //    return true;
        //CTS07_id or CTS08_id
        //ModLog.Info(__instance.closeFadeWindow.Value);
        //ModLog.Info(__instance.waitForFinalFade.Value);
        //__instance.closeFadeWindow.Value = true;
        //__instance.waitForFinalFade.Value = false;

        string name = __instance.cutsceneId?.name ?? "Invalid";

        if (BANNED_CUTSCENES.Contains(name))
            return true;

        if (FADE_CUTSCENES.Contains(name))
            t4.FADE_FLAG = true;

        ModLog.Warn("Skipping cutscene: " + name);
        __instance.Finish();
        return false;
    }

    private static readonly string[] BANNED_CUTSCENES = ["CTS17_id"];
    private static readonly string[] FADE_CUTSCENES = ["CTS08_id"];
}


//[HarmonyPatch(typeof(CutsceneManager), nameof(CutsceneManager.PlayCutsceneAsync), typeof(CutsceneID), typeof(GameObject), typeof(bool), typeof(float), typeof(Color), typeof(float), typeof(float), typeof(Color), typeof(float), typeof(Color), typeof(float), typeof(Color), typeof(bool), typeof(bool), typeof(CancellationToken))]
//class CutsceneManager_PlayCutsceneAsync_Patch
//{
//    public static bool Prefix(CutsceneManager __instance, ref CutsceneID cutscene)
//    {
//        ModLog.Info("Starting cutscene " + cutscene.name);
//        cutscene = null;
//        return false;
//    }
//}

/// <summary>
/// Skip quotes from playmaker
/// </summary>
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

[HarmonyPatch(typeof(SetFadeColorAction), nameof(SetFadeColorAction.OnEnter))]
class t6
{
    public static void Prefix(SetFadeColorAction __instance)
    {
        ModLog.Error("Set color: " + __instance.color.Value.ToString());
    }
}

//[HarmonyPatch(typeof(CutsceneManager), nameof(CutsceneManager.FadeAndClose))]
//class t
//{
//    public static bool Prefix(CutsceneManager __instance, ref Color fadeOut2Color)
//    {
//        ModLog.Info("FadeAndClose");
//        fadeOut2Color = Color.black;
//        return true;
//    }
//}

//[HarmonyPatch(typeof(CutsceneManager), nameof(CutsceneManager.FadeAndCloseAsync))]
//class t2
//{
//    public static bool Prefix(CutsceneManager __instance, ref Color fadeOut2Color)
//    {
//        ModLog.Info("FadeAndCloseAsync");
//        fadeOut2Color = Color.black;
//        return true;
//    }
//}

//[HarmonyPatch(typeof(FadeWindowLogic), nameof(FadeWindowLogic.FadeAsync), typeof(float), typeof(Action), typeof(Color), typeof(CancellationToken))]
//class t3
//{
//    public static bool Prefix(FadeWindowLogic __instance, ref Color targetColor)
//    {
//        ModLog.Info("FadeAsync with cancel: " + targetColor);
//        //targetColor = Color.black;
//        return true;
//    }
//}

[HarmonyPatch(typeof(FadeWindowLogic), nameof(FadeWindowLogic.FadeAsync), typeof(float), typeof(Action), typeof(Color))]
class t4
{
    public static bool Prefix(FadeWindowLogic __instance, ref Color targetColor)
    {
        ModLog.Info("FadeAsync no cancel: " + targetColor);

        if (FADE_FLAG)
        {
            ModLog.Info("Forcing fade to black");
            targetColor = Color.black;
            FADE_FLAG = false;
        }

        return true;
    }

    public static bool FADE_FLAG { get; set; }
}

internal static class InfoExtensions
{
    // Recursive method that returns the entire hierarchy of an object
    public static string DisplayHierarchy(this Transform transform, int maxLevel, bool includeComponents)
    {
        return transform.DisplayHierarchy_INTERNAL(new StringBuilder(), 0, maxLevel, includeComponents).ToString();
    }

    public static StringBuilder DisplayHierarchy_INTERNAL(this Transform transform, StringBuilder currentHierarchy, int currentLevel, int maxLevel, bool includeComponents)
    {
        // Indent
        for (int i = 0; i < currentLevel; i++)
            currentHierarchy.Append('\t');

        // Add this object
        currentHierarchy.Append(transform.name);

        // Add components
        if (includeComponents)
        {
            currentHierarchy.Append(" - ");
            foreach (Component c in transform.GetComponents<Component>())
                currentHierarchy.Append(c.ToString() + ", ");
        }
        currentHierarchy.AppendLine();

        // Add children
        if (currentLevel < maxLevel)
        {
            for (int i = 0; i < transform.childCount; i++)
                currentHierarchy = transform.GetChild(i).DisplayHierarchy_INTERNAL(currentHierarchy, currentLevel + 1, maxLevel, includeComponents);
        }

        // Return output
        return currentHierarchy;
    }

    // Displays all states and actions of a playmaker fsm
    public static void DisplayActions(this PlayMakerFSM fsm)
    {
        ModLog.Warn("FSM: " + fsm.name);
        foreach (var state in fsm.FsmStates)
        {
            ModLog.Info("State: " + state.Name);
            foreach (var action in state.Actions)
            {
                ModLog.Error("Action: " + action.Name + ", " + action.GetIl2CppType().Name);
            }
        }
    }
}