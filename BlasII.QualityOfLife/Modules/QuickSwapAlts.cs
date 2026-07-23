using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Abilities;
using Il2CppTGK.Game.Components.Attack.Data;
using System.Collections.Generic;
using UnityEngine;

namespace BlasII.QualityOfLife.Modules;

internal class QuickSwapAlts_9 : BaseModule
{
    public override void OnUpdate()
    {
        if (!Main.QualityOfLife.CurrentSettings.QuickSwapAlts)
            return;

        if (!Main.QualityOfLife.InputHandler.GetButton(ModdingAPI.Input.ButtonType.Interact))
            return;

        //if (Main.QualityOfLife.InputHandler.GetButtonDown(ModdingAPI.Input.ButtonType.ChangeWeapon))
        if (!Input.GetKeyDown(KeyCode.R))
            return;

        ModLog.Warn("quick swap");

        WeaponID current = CoreCache.EquipmentManager.GetCurrentWeapon();
        ModLog.Error(current.name);

        if (current == AssetStorage.Weapons[WEAPON_IDS.Censer])
        {
            // Swap from censer to whip
            WeaponID whip = AssetStorage.Weapons[WEAPON_IDS.Whip];

            if (!CoreCache.EquipmentManager.HasWeapon(whip))
                return;

            ModLog.Info($"Quickswapping from {current.name} to {whip.name}");
            CoreCache.EquipmentManager.SwapWeapons(current, whip);
            Object.FindObjectOfType<ChangeWeaponAbility>().ChangeWeapon(whip);
        }
    }
}
