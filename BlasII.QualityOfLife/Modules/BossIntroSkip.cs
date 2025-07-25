﻿using HarmonyLib;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Game;

namespace BlasII.QualityOfLife.Modules;

internal class BossIntroSkip_3 : BaseModule { }

/// <summary>
/// Force boss intro flags
/// </summary>
[HarmonyPatch(typeof(QuestManager), nameof(QuestManager.GetQuestVarBoolValue))]
class QuestManager_GetVarBool_Patch_BIS
{
    public static void Postfix(int questId, ref bool __result)
    {
        var quest = CoreCache.Quest.GetQuestData(questId, string.Empty);

        if (quest.Name == "BossesIntro" && Main.QualityOfLife.CurrentSettings.BossIntroSkip)
            __result = true;
    }
}
