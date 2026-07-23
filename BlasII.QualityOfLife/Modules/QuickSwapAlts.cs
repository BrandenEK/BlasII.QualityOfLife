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
        if (Main.QualityOfLife.InputHandler.GetButton(ModdingAPI.Input.ButtonType.Interact))
        {
            //ModLog.Error("Hold interact");

            if (UnityEngine.Input.GetKeyDown(KeyCode.R))
            //if (Main.QualityOfLife.InputHandler.GetButtonDown(ModdingAPI.Input.ButtonType.ChangeWeapon))
            {
                ModLog.Warn("quick swap");
                Object.FindObjectOfType<ChangeWeaponAbility>().ChangeWeapon(AssetStorage.Weapons[WEAPON_IDS.Whip]);
                CoreCache.EquipmentManager.SwapWeapons(AssetStorage.Weapons[WEAPON_IDS.Censer], AssetStorage.Weapons[WEAPON_IDS.Whip]);

            }
        }
    }
}
