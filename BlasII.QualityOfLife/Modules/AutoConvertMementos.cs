using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using HarmonyLib;
using Il2CppPlaymaker.Inventory;
using Il2CppTGK.Game.Components.Inventory;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace BlasII.QualityOfLife.Modules;

internal class AutoConvertMementos : BaseModule
{
    public override string Name { get; } = "AutoConvertMementos";
    public override int Order { get; } = 6;
}

/// <summary>
/// If a remembrance item was given on this frame, replace it with the figure
/// </summary>
[HarmonyPatch(typeof(InventoryComponent), nameof(InventoryComponent.AddItemAsync))]
class InventoryComponent_AddItemAsync_Patch_ACM
{
    public static void Prefix(ref ItemID itemID)
    {
        if (!Main.QualityOfLife.CurrentSettings.AutoConvertMementos)
            return;

        if (Time.frameCount != ITEM_FRAME)
            return;

        if (!REMEMBRANCE_ITEMS.TryGetValue(itemID.name, out string newItem))
            return;

        ModLog.Info($"Replacing {itemID.name} with {newItem}");
        itemID = AssetStorage.Figures[newItem];
    }

    // When an item should be added, store the frameCount and if a memento is trying to be added on the same frame, replace it
    public static int ITEM_FRAME { get; set; }

    private static readonly Dictionary<string, string> REMEMBRANCE_ITEMS = new()
    {
        { "QI04", "FG19" },
        { "QI06", "FG09" },
        { "QI09", "FG23" },
        { "QI10", "FG20" },
        { "QI55", "FG29" },
        { "QI62", "FG10" },
        { "QI71", "FG44" },
    };
}

/// <summary>
/// Replace items given through playmaker
/// </summary>
[HarmonyPatch(typeof(AddItem), nameof(AddItem.OnEnter))]
class AddItem_OnEnter_Patch_ACM
{
    public static void Prefix()
    {
        InventoryComponent_AddItemAsync_Patch_ACM.ITEM_FRAME = Time.frameCount;
    }
}

/// <summary>
/// Replace items given through shops
/// </summary>
[HarmonyPatch(typeof(ShopManager), nameof(ShopManager.SellItem))]
class ShopManager_SellItem_Patch_ACM
{
    public static void Prefix()
    {
        InventoryComponent_AddItemAsync_Patch_ACM.ITEM_FRAME = Time.frameCount;
    }
}


[HarmonyPatch(typeof(InventoryComponent), nameof(InventoryComponent.CanUnequipItem))]
class t
{
    public static bool Prefix(ref bool __result)
    {
        __result = true;
        return false;
    }
}

[HarmonyPatch(typeof(PR17PopupWindowLogic), nameof(PR17PopupWindowLogic.ShowPopup))]
class t2
{
    public static void Postfix()
    {
        ModLog.Warn("Opening popup!");
    }
}
// Randomizer GiveItem() patch ??
