﻿using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppPlaymaker.UI;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Game.Tutorial;

namespace BlasII.QualityOfLife.Modules;

internal class TutorialSkip_2 : BaseModule { }

/// <summary>
/// Skip tutorials from playmaker
/// </summary>
[HarmonyPatch(typeof(ShowTutorial), nameof(ShowTutorial.OnEnter))]
internal class ShowTutorial_OnEnter_Patch_TS
{
    public static bool Prefix(ShowTutorial __instance)
    {
        if (!Main.QualityOfLife.CurrentSettings.TutorialSkip)
            return true;

        TutorialID tutorial = __instance.tutorial.Value.Cast<TutorialID>();
        ModLog.Warn("Skipping tutorial: " + tutorial?.name);
        __instance.Finish();
        return false;
    }
}

/// <summary>
/// Skip tutorials from the manager
/// </summary>
[HarmonyPatch(typeof(TutorialManager), nameof(TutorialManager.ShowTutorialAsync))]
internal class TutorialManager_ShowTutorialAsync_Patch_TS
{
    public static bool Prefix(TutorialID tutorialID)
    {
        if (!Main.QualityOfLife.CurrentSettings.TutorialSkip)
            return true;

        ModLog.Warn("Skipping tutorial: " + tutorialID.name);
        return false;
    }
}

/// <summary>
/// Force tutorial flags
/// </summary>
[HarmonyPatch(typeof(QuestManager), nameof(QuestManager.GetQuestVarBoolValue))]
class QuestManager_GetVarBool_Patch_TS
{
    public static void Postfix(int questId, ref bool __result)
    {
        var quest = CoreCache.Quest.GetQuestData(questId, string.Empty);

        if (quest.Name == "Tutorials" && Main.QualityOfLife.CurrentSettings.TutorialSkip)
            __result = true;
    }
}
