using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Abilities;
using Il2CppTGK.Game.Components.Attack.Data;
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

        if (!Main.QualityOfLife.InputHandler.GetButtonDown(ModdingAPI.Input.ButtonType.NextWeapon))
            return;

        WeaponID current = CoreCache.EquipmentManager.GetCurrentWeapon();

        if (current == AssetStorage.Weapons[WEAPON_IDS.Censer])
            TryPerformSwap(current, AssetStorage.Weapons[WEAPON_IDS.Whip]);
        else if (current == AssetStorage.Weapons[WEAPON_IDS.Whip])
            TryPerformSwap(current, AssetStorage.Weapons[WEAPON_IDS.Censer]);
        else if (current == AssetStorage.Weapons[WEAPON_IDS.RosaryBlade])
            TryPerformSwap(current, AssetStorage.Weapons[WEAPON_IDS.MeaCulpa]);
        else if (current == AssetStorage.Weapons[WEAPON_IDS.MeaCulpa])
            TryPerformSwap(current, AssetStorage.Weapons[WEAPON_IDS.RosaryBlade]);
    }

    private void TryPerformSwap(WeaponID current, WeaponID next)
    {
        if (!CoreCache.EquipmentManager.HasWeapon(next))
            return;

        ModLog.Info($"Quickswapping from {current.name} to {next.name}");
        CoreCache.EquipmentManager.SwapWeapons(current, next);
        Object.FindObjectOfType<ChangeWeaponAbility>().FastChange(next);
    }
}
