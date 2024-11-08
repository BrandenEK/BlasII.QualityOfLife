using BlasII.ModdingAPI.Assets;
using BlasII.ModdingAPI.Input;
using Il2CppLightbug.Kinematic2D.Implementation;
using Il2CppTGK.Game;

namespace BlasII.QualityOfLife;

internal class MirabrasGlitches
{
    public void Update()
    {
        if (!Main.QualityOfLife.CurrentSettings.AllowMirabrasGlitches)
            return;

        if (Main.QualityOfLife.InputHandler.GetButtonDown(ButtonType.NextWeapon))
        {
            var changeWeapon = AssetStorage.Abilities[ABILITY_IDS.ChangeWeapon];
            var fullPrayer = AssetStorage.Abilities[ABILITY_IDS.FullPrayer];

            var controller = CoreCache.PlayerSpawn.PlayerInstance.GetComponent<CharacterController2D>();
            controller.CancelAbility(fullPrayer);
            controller.ActivateAbilityByType(changeWeapon);
        }
    }
}
