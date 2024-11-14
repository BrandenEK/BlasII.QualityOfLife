using BlasII.ModdingAPI.Assets;
using BlasII.ModdingAPI.Input;
using Il2CppLightbug.Kinematic2D.Implementation;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Abilities;
using Il2CppTGK.Game.Components.Attack.Data;

namespace BlasII.QualityOfLife;

internal class MirabrasGlitches
{
    public void Update()
    {
        if (!Main.QualityOfLife.CurrentSettings.AllowMirabrasGlitches)
            return;

        if (!Main.QualityOfLife.InputHandler.GetButtonDown(ButtonType.NextWeapon))
            return;

        if (CoreCache.EquipmentManager.CountUnlockedWeapons() < 2)
            return;

        var changeWeapon = AssetStorage.Abilities[ABILITY_IDS.ChangeWeapon];
        var fullPrayer = AssetStorage.Abilities[ABILITY_IDS.FullPrayer];

        var controller = CoreCache.PlayerSpawn.PlayerInstance.GetComponent<CharacterController2D>();
        controller.CancelAbility(fullPrayer);
        controller.ActivateAbilityByType(changeWeapon);

        WeaponID currentWeapon = CoreCache.EquipmentManager.GetCurrentWeapon();
        int nextWeaponSlot = CoreCache.EquipmentManager.GetWeaponSlot(currentWeapon) + 1;
        if (nextWeaponSlot >= CoreCache.EquipmentManager.GetNumWeaponSlots())
            nextWeaponSlot = 0;
        WeaponID nextWeapon = CoreCache.EquipmentManager.GetAssignedWeaponToSlot(nextWeaponSlot);

        CoreCache.PlayerSpawn.PlayerControllerRef.GetAbility<ChangeWeaponAbility>().ChangeWeapon(nextWeapon);
    }
}
