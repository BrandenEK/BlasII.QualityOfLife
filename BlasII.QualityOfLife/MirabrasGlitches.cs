using BlasII.ModdingAPI.Input;
using BlasII.ModdingAPI.Storage;
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
                AbilityStorage.TryGetAbility("AB10", out var changeWeapon);
                AbilityStorage.TryGetAbility("AB21", out var fullPrayer);

                var controller = CoreCache.PlayerSpawn.PlayerInstance.GetComponent<CharacterController2D>();
                controller.CancelAbility(fullPrayer);
                controller.ForceAbilityExecution(changeWeapon);
            }
        }
    }
}
