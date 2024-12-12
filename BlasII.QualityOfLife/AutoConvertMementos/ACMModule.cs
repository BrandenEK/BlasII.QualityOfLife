using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using HarmonyLib;
using Il2CppTGK.Game.Components.Inventory;
using Il2CppTGK.Inventory;
using System.Collections.Generic;

namespace BlasII.QualityOfLife.AutoConvertMementos;

internal class ACMModule : BaseModule
{
    public override string Name { get; } = "AutoConvertMementos";
    public override int Order { get; } = 6;
}

[HarmonyPatch(typeof(InventoryComponent), nameof(InventoryComponent.AddItemAsync))]
class InventoryComponent_AddItemAsync_Patch
{
    public static void Prefix(ref ItemID itemID)
    {
        if (!Main.QualityOfLife.CurrentSettings.AutoConvertMementos)
            return;

        if (!REMEMBRANCE_ITEMS.TryGetValue(itemID.name, out string newItem))
            return;

        ModLog.Info($"Replacing {itemID.name} with {newItem}");
        itemID = AssetStorage.Figures[newItem];
    }

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