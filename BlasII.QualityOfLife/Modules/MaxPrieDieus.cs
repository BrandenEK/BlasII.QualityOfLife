using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppPlaymaker.PrieDieu;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Game.PrieDieu;
using System.Linq;

namespace BlasII.QualityOfLife.Modules;

internal class MaxPrieDieus : BaseModule
{
    public override string Name { get; } = "MaxPrieDieus";
    public override int Order { get; } = 8;
}

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

        if (upgradeID.name == "TeleportToHUBUpgrade")
            return;

        __result = true;
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

/// <summary>
/// Skip the first three upgrades from the Cobijadas
/// </summary>
[HarmonyPatch(typeof(IsPrieDieuUpgraded), nameof(IsPrieDieuUpgraded.OnEnter))]
class IsPrieDieuUpgraded_OnEnter_Patch_MPD
{
    public static bool Prefix(IsPrieDieuUpgraded __instance)
    {
        if (!Main.QualityOfLife.CurrentSettings.MaxPrieDieus)
            return true;

        ModLog.Info($"Skipping prieu dieu upgrade: {__instance.prieDieuUpgrade.Value.name}");
        __instance.Fsm.Event(__instance.yesEvent);
        __instance.Finish();
        return false;
    }
}
