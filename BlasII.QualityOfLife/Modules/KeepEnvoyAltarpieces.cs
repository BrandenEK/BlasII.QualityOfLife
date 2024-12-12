using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppPlaymaker.Inventory;
using System.Linq;

namespace BlasII.QualityOfLife.Modules;

internal class KeepEnvoyAltarpieces : BaseModule
{
    public override string Name { get; } = "KeepEnvoyAltarpieces";
    public override int Order { get; } = 5;
}

/// <summary>
/// Prevent removal of FG30-FG33
/// </summary>
[HarmonyPatch(typeof(RemoveItem), nameof(RemoveItem.OnEnter))]
class RemoveItem_OnEnter_Patch_KEA
{
    public static bool Prefix(RemoveItem __instance)
    {
        string itemId = __instance.itemID.name;

        if (!Main.QualityOfLife.CurrentSettings.KeepEnvoyAltarpieces || !REMOVAL_ITEMS.Contains(itemId))
            return true;

        ModLog.Warn($"Preventing removal of {itemId}");
        __instance.Finish();
        return false;
    }

    private static readonly string[] REMOVAL_ITEMS = ["FG30", "FG31", "FG32", "FG33"];
}

/// <summary>
/// Prevent addition of FG40-FG43
/// </summary>
[HarmonyPatch(typeof(AddItem), nameof(AddItem.OnEnter))]
class AddItem_OnEnter_Patch_KEA
{
    public static bool Prefix(AddItem __instance)
    {
        string itemId = __instance.itemID.name;

        if (!Main.QualityOfLife.CurrentSettings.KeepEnvoyAltarpieces || !ADDITION_ITEMS.Contains(itemId))
            return true;

        ModLog.Warn($"Preventing addition of {itemId}");
        __instance.Finish();
        return false;
    }

    private static readonly string[] ADDITION_ITEMS = ["FG40", "FG41", "FG42", "FG43"];
}
