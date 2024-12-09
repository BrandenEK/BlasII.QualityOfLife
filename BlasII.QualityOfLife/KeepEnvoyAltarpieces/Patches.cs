using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppPlaymaker.Inventory;
using System.Linq;

namespace BlasII.QualityOfLife.KeepEnvoyAltarpieces;

/// <summary>
/// Prevent removal of FG30-FG33
/// </summary>
[HarmonyPatch(typeof(RemoveItem), nameof(RemoveItem.OnEnter))]
class RemoveItem_OnEnter_Patch
{
    public static bool Prefix(RemoveItem __instance)
    {
        string itemId = __instance.itemID.name;

        if (!REMOVAL_ITEMS.Contains(itemId))
            return true;

        ModLog.Error($"Preventing removal of {itemId}");
        __instance.Finish();
        return false;
    }

    private static readonly string[] REMOVAL_ITEMS = ["FG30", "FG31", "FG32", "FG33"];
}

/// <summary>
/// Prevent addition of FG40-FG43
/// </summary>
[HarmonyPatch(typeof(AddItem), nameof(AddItem.OnEnter))]
class AddItem_OnEnter_Patch
{
    public static bool Prefix(AddItem __instance)
    {
        string itemId = __instance.itemID.name;

        if (!ADDITION_ITEMS.Contains(itemId))
            return true;

        ModLog.Error($"Preventing addition of {itemId}");
        __instance.Finish();
        return false;
    }

    private static readonly string[] ADDITION_ITEMS = ["FG40", "FG41", "FG42", "FG43"];
}