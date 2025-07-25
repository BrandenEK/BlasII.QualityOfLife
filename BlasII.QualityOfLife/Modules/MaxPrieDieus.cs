﻿using HarmonyLib;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Game.PrieDieu;
using System.Linq;

namespace BlasII.QualityOfLife.Modules;

internal class MaxPrieDieus_8 : BaseModule { }

/// <summary>
/// Always have all the PD upgrades
/// </summary>
[HarmonyPatch(typeof(PrieDieuManager), nameof(PrieDieuManager.IsUpgraded))]
class PrieDieuManager_IsUpgraded_Patch_MPD
{
    public static void Postfix(PrieDieuUpgradeID upgradeID, ref bool __result)
    {
        if (!Main.QualityOfLife.CurrentSettings.MaxPrieDieus)
            return;

        __result = true;
    }
}
[HarmonyPatch(typeof(PrieDieuManager), nameof(PrieDieuManager.GetPrieDieuLevel))]
class PrieDieuManager_GetPrieDieuLevel_Patch
{
    public static void Postfix(ref int __result)
    {
        if (!Main.QualityOfLife.CurrentSettings.MaxPrieDieus)
            return;

        __result = 3;
    }
}

/// <summary>
/// Skip the first three upgrades from the Cobijadas
/// </summary>
[HarmonyPatch(typeof(QuestManager), nameof(QuestManager.GetQuestVarBoolValue))]
class QuestManager_GetQuestVarBoolValue_Patch_MPD
{
    public static void Postfix(int questId, int varId, ref bool __result)
    {
        if (!Main.QualityOfLife.CurrentSettings.MaxPrieDieus)
            return;

        var quest = CoreCache.Quest.GetQuestData(questId, string.Empty);
        var variable = quest.GetVariable(varId);

        if (quest.Name != "ST25" || !Enumerable.Range(1, 3).Any(x => variable.id == $"UPGRADE{x}_UNLOCKED"))
            return;

        __result = true;
    }
}
