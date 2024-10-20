using HarmonyLib;
using Il2CppPlaymaker.UI;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Cutscenes;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Game.Tutorial;

namespace BlasII.QualityOfLife;

/// <summary>
/// Level 1 - Skip tutorials
/// </summary>
[HarmonyPatch(typeof(ShowTutorial), nameof(ShowTutorial.OnEnter))]
internal class Tutorial_Skip1_Patch
{
    public static bool Prefix(ShowTutorial __instance)
    {
        if (!QualityOfLife.IsModuleLevelActive("Skip_Story_Level", 1))
            return true;

        TutorialID tutorial = __instance.tutorial.Value.Cast<TutorialID>();
        Main.QualityOfLife.LogWarning("Skipping tutorial: " + tutorial?.name);
        __instance.Finish();
        return false;
    }
}
[HarmonyPatch(typeof(TutorialManager), nameof(TutorialManager.ShowTutorialAsync))]
internal class Tutorial_Skip2_Patch
{
    public static bool Prefix(TutorialID tutorialID)
    {
        if (!QualityOfLife.IsModuleLevelActive("Skip_Story_Level", 1))
            return true;

        Main.QualityOfLife.LogWarning("Skipping tutorial: " + tutorialID.name);
        return false;
    }
}

/// <summary>
/// Level 3 - Skip cutscenes
/// </summary>
[HarmonyPatch(typeof(PlayCutscene), nameof(PlayCutscene.OnEnter))]
class Cutscene_Skip_Patch
{
    public static bool Prefix(PlayCutscene __instance)
    {
        if (!QualityOfLife.IsModuleLevelActive("Skip_Story_Level", 3))
            return true;

        // Don't skip Eviterno defeat cutscene
        if (__instance.cutsceneId?.name == "CTS17_id")
            return true;

        Main.QualityOfLife.LogWarning("Skipping cutscene: " + __instance.cutsceneId?.name);
        __instance.Finish();
        return false;
    }
}
[HarmonyPatch(typeof(ShowQuote), nameof(ShowQuote.OnEnter))]
class Quote_Skip_Patch
{
    public static bool Prefix(ShowQuote __instance)
    {
        if (!QualityOfLife.IsModuleLevelActive("Skip_Story_Level", 3))
            return true;

        Main.QualityOfLife.LogWarning("Skipping quote: " + __instance.Owner.name);
        __instance.Finish();
        return false;
    }
}

/// <summary>
/// Level 4 - Skip story events
/// </summary>
[HarmonyPatch(typeof(ShowMapDestinationTutorial), nameof(ShowMapDestinationTutorial.OnEnter))]
class Map_Skip_Patch
{
    public static bool Prefix(ShowMapDestinationTutorial __instance)
    {
        if (!QualityOfLife.IsModuleLevelActive("Skip_Story_Level", 4))
            return true;

        Main.QualityOfLife.LogWarning("Skipping map event: " + __instance.Owner.name);
        __instance.Finish();
        return false;
    }
}
[HarmonyPatch(typeof(ShowSorrowsPopup), nameof(ShowSorrowsPopup.OnEnter))]
class Sorrows_Skip_Patch
{
    public static bool Prefix(ShowSorrowsPopup __instance)
    {
        if (!QualityOfLife.IsModuleLevelActive("Skip_Story_Level", 4))
            return true;

        Main.QualityOfLife.LogWarning("Skipping sorrows event: " + __instance.Owner.name);
        __instance.Finish();
        return false;
    }
}
[HarmonyPatch(typeof(ShowDovePopup), nameof(ShowDovePopup.OnEnter))]
class Dove_Skip_Patch
{
    public static bool Prefix(ShowDovePopup __instance)
    {
        if (!QualityOfLife.IsModuleLevelActive("Skip_Story_Level", 4))
            return true;

        Main.QualityOfLife.LogWarning("Skipping dove event: " + __instance.Owner.name);
        __instance.Finish();
        return false;
    }
}

/// <summary>
/// Various levels - Force flags
/// </summary>
[HarmonyPatch(typeof(QuestManager), nameof(QuestManager.GetQuestVarBoolValue))]
class QuestManager_GetVarBool_Patch
{
    public static void Postfix(int questId, ref bool __result)
    {
        var quest = CoreCache.Quest.GetQuestData(questId, string.Empty);

        // Level 1 - Skip tutorials
        if (quest.Name == "Tutorials" && QualityOfLife.IsModuleLevelActive("Skip_Story_Level", 1))
            __result = true;

        // Level 2 - Skip boss intros
        if (quest.Name == "BossesIntro" && QualityOfLife.IsModuleLevelActive("Skip_Story_Level", 2))
            __result = true;
    }
}
