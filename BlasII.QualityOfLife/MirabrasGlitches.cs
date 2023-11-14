using BlasII.ModdingAPI.Input;
using BlasII.ModdingAPI.Assets;
using Il2CppLightbug.Kinematic2D.Implementation;
using Il2CppTGK.Game;

namespace BlasII.QualityOfLife
{
    internal class MirabrasGlitches
    {
        public void Update()
        {
            if (!Main.QualityOfLife.QolSettings.allowMirabrasGlitches)
                return;

            if (Main.QualityOfLife.InputHandler.GetButtonDown(ButtonType.NextWeapon))
            {
                var changeWeapon = AssetStorage.Abilities["AB10"];
                var fullPrayer = AssetStorage.Abilities["AB21"];

                var controller = CoreCache.PlayerSpawn.PlayerInstance.GetComponent<CharacterController2D>();
                controller.CancelAbility(fullPrayer);
                controller.ActivateAbilityByType(changeWeapon);
            }
        }
    }
}
