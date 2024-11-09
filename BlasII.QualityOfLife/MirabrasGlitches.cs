using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using BlasII.ModdingAPI.Input;
using HarmonyLib;
using Il2CppLightbug.Kinematic2D.Implementation;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Managers;

namespace BlasII.QualityOfLife;

internal class MirabrasGlitches
{
    private bool next = false;
    private int frames = 255;

    public void Update()
    {
        if (!Main.QualityOfLife.CurrentSettings.AllowMirabrasGlitches)
            return;

        if (frames > 0 && frames != 255)
        {
            frames--;

            var changeWeapon = AssetStorage.Abilities[ABILITY_IDS.ChangeWeapon];

            var controller = CoreCache.PlayerSpawn.PlayerInstance.GetComponent<CharacterController2D>();
            controller.ActivateAbilityByType(changeWeapon);
        }
        //ModLog.Warn("Frames: " + frames);

        //if (frames == 0)
        //{
        //    next = false;
        //    frames = 255;
        //    var changeWeapon = AssetStorage.Abilities[ABILITY_IDS.ChangeWeapon];
        //    var fullPrayer = AssetStorage.Abilities[ABILITY_IDS.FullPrayer];

        //    var controller = CoreCache.PlayerSpawn.PlayerInstance.GetComponent<CharacterController2D>();
        //    controller.CancelAbility(fullPrayer);
        //    controller.ActivateAbilityByType(changeWeapon);
        //}

        if (Main.QualityOfLife.InputHandler.GetButtonDown(ButtonType.NextWeapon))
        {
            var changeWeapon = AssetStorage.Abilities[ABILITY_IDS.ChangeWeapon];
            var fullPrayer = AssetStorage.Abilities[ABILITY_IDS.FullPrayer];

            ModLog.Info("Unlocked: " + (CoreCache.EquipmentManager.CountUnlockedWeapons() > 1));
            if (CoreCache.EquipmentManager.CountUnlockedWeapons() < 2)
                return;

            var controller = CoreCache.PlayerSpawn.PlayerInstance.GetComponent<CharacterController2D>();
            controller.CancelAbility(fullPrayer);
            controller.ActivateAbilityByType(changeWeapon);

            next = true;
            frames = 10;
        }
    }
}

[HarmonyPatch(typeof(AbilityLockManager), nameof(AbilityLockManager.SetAbility))]
class t
{
    public static void Postfix(IAbilityTypeRef abilityID, bool status)
    {
        ModLog.Error($"Setting {abilityID.name} to {status}");
    }
}