﻿using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using HarmonyLib;
using Il2CppPlaymaker.Inventory;
using Il2CppTGK.Game.Components.Inventory;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace BlasII.QualityOfLife.AutoConvertMementos;

internal class ACMModule : BaseModule
{
    public override string Name { get; } = "AutoConvertMementos";
    public override int Order { get; } = 6;

    public static readonly Dictionary<string, string> REMEMBRANCE_ITEMS = new()
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

[HarmonyPatch(typeof(InventoryComponent), nameof(InventoryComponent.AddItemAsync))]
class InventoryComponent_AddItemAsync_Patch
{
    public static void Prefix(ref ItemID itemID)
    {
        //if (!Main.QualityOfLife.CurrentSettings.AutoConvertMementos)
        //    return;
        ModLog.Warn("Frame: " + Time.frameCount);
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
class AddItem_OnEnter_Patch
{
    public static void Prefix(AddItem __instance)
    {
        if (!Main.QualityOfLife.CurrentSettings.AutoConvertMementos)
            return;
        ModLog.Error("Frame: " + Time.frameCount);
        InventoryComponent_AddItemAsync_Patch.ITEM_FRAME = Time.frameCount;
        //if (!ACMModule.REMEMBRANCE_ITEMS.TryGetValue(__instance.itemID.name, out string newItem))
        //    return;

        //ModLog.Info($"Replacing {__instance.itemID.name} with {newItem}");
        //__instance.itemID = AssetStorage.Figures[newItem];
    }
}

/// <summary>
/// Replace items given through shops
/// </summary>
[HarmonyPatch(typeof(ShopManager), nameof(ShopManager.SellItem))]
class ShopManager_SellItem_Patch
{
    public static void Prefix(ItemID itemId)
    {
        if (!Main.QualityOfLife.CurrentSettings.AutoConvertMementos)
            return;
        ModLog.Error("Frame: " + Time.frameCount);
        InventoryComponent_AddItemAsync_Patch.ITEM_FRAME = Time.frameCount;
        //if (!ACMModule.REMEMBRANCE_ITEMS.TryGetValue(itemId.name, out string newItem))
        //    return;

        //ModLog.Info($"Replacing {itemId.name} with {newItem}");
        //AssetStorage.PlayerInventory.RemoveItem(itemId);
        //AssetStorage.PlayerInventory.AddItemAsync(AssetStorage.Figures[newItem]);
    }
}
