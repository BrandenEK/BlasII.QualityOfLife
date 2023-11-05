using BlasII.ModdingAPI.Storage;
using HarmonyLib;
using Il2CppLightbug.Kinematic2D.Implementation;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Abilities;
using Il2CppTGK.Game.Components.Prayers;
using UnityEngine;

namespace BlasII.QualityOfLife
{
    internal class MirabrasGlitches
    {
        public void Update()
        {
            if (CoreCache.Input.GetButtonDown("Next Weapon"))
            {
                Main.QualityOfLife.LogWarning("Pressed r");

                AbilityStorage.TryGetAbility("AB10", out var changeWeapon);
                AbilityStorage.TryGetAbility("AB21", out var fullPrayer);

                var controller = CoreCache.PlayerSpawn.PlayerInstance.GetComponent<CharacterController2D>();
                controller.CancelAbility(fullPrayer);
                controller.ForceAbilityExecution(changeWeapon);
            }

            if (Input.GetKeyDown(KeyCode.F7))
            {
                Main.QualityOfLife.LogWarning("Trying custom switch weapons");

                WeaponStorage.TryGetWeapon("WE02", out var weapon);
                //Object.FindObjectOfType<ChangeWeaponAbility>().OnFinalizeExecution();
                //Object.FindObjectOfType<ChangeWeaponAbility>().OnCancel();
                //CoreCache.Input.ClearAllInputBlocks();
                //Object.FindObjectOfType<ChangeWeaponAbility>().OnAbilityEnabled();
                //Object.FindObjectOfType<ChangeWeaponAbility>().OnStartConcurrentExecution();

                AbilityStorage.TryGetAbility("AB10", out var changeWeapon);
                AbilityStorage.TryGetAbility("AB21", out var fullPrayer);

                var controller = Object.FindObjectOfType<CharacterController2D>();
                var changer = Object.FindObjectOfType<ChangeWeaponAbility>();

                foreach (var ability in AbilityStorage.GetAllAbilities())
                {
                    controller.UnApplyBlockTable(ability.Value);
                }

                controller.CancelAbility(fullPrayer);
                controller.ForceAbilityExecution(changeWeapon);
                //changer.ChangeWeapon(weapon);

                //controller.UnApplyBlockTable(changeWeapon);
                //controller.UnApplyBlockTable(fullPrayer);
                //controller.UnBlockAll(true);
                //controller.ActivateAbilityByType(ability);
                //controller.PerformInterruption(Object.FindObjectOfType<ChangeWeaponAbility>());
                //Object.FindObjectOfType<CharacterController2D>().SetActiveAbilityByType(ability, true);
                //Object.FindObjectOfType<CharacterController2D>().ForceAbilityExecution(ability);
                //Object.FindObjectOfType<CharacterController2D>().UnBlockAbility<ChangeWeaponAbility>(ability, true);


                //Object.FindObjectOfType<ChangeWeaponAbility>().RequestFastChangeNextWeapon();
                //Object.FindObjectOfType<ChangeWeaponAbility>().OnStartExecution();

                //var table = Resources.FindObjectsOfTypeAll<AbilitiesBlockTable>()[0];
                //for (int i = 0; i < table.blockEntries.Length; i++)
                //{
                //    var entry = table.blockEntries[i];
                //    LogWarning(entry.ability.name);
                //    for (int j = 0; j < entry.abilitiesAffected.Length; j++)
                //    {
                //        LogError(entry.abilitiesAffected[j].name);
                //    }
                //}

                //var table = Resources.FindObjectsOfTypeAll<AbilitiesConcurrentTable>()[0];
                //Main.Randomizer.Log("Default: " + table.concurrentByDefault);
                //for (int i = 0; i < table.concurrentEntries.Length; i++)
                //{
                //    var entry = table.concurrentEntries[i];
                //    LogWarning(entry.ability.name);
                //    for (int j = 0; j < entry.abilitiesAffected.Length; j++)
                //    {
                //        LogError(entry.abilitiesAffected[j].name);
                //    }
                //}
            }
        }
    }

    [HarmonyPatch(typeof(ChangeWeaponAbility), nameof(ChangeWeaponAbility.RequestFastChangeNextWeapon))]
    class t1
    {
        public static void Postfix() => Main.QualityOfLife.Log("Change weapon request");
    }

    //[HarmonyPatch(typeof(FullPrayerAbility), nameof(FullPrayerAbility.CanBeExecutedConcurrentlyWith))]
    //class t2
    //{
    //    public static void Postfix(ICharacterAbility ability) => Main.Randomizer?.Log("Concurrent: " + ability?.GetAbilityID());
    //}

    //    [HarmonyPatch(typeof(FullPrayerAbility), nameof(FullPrayerAbility.CanBeInterruptedBy))]
    //    class t3
    //    {
    //        public static void Postfix(ICharacterAbility interruptor) => Main.Randomizer?.Log("Interrupt: " + interruptor?.GetAbilityID());
    //    }

    //[HarmonyPatch(typeof(FullPrayerAbility), nameof(FullPrayerAbility.CanBeInterruptedBy))]
    //class t4
    //{
    //    public static bool Prefix(ref bool __result)
    //    {
    //        Main.Randomizer.LogWarning("Interrupt");
    //        __result = true;
    //        return false;
    //    }
    //}

    //[HarmonyPatch(typeof(FullPrayerAbility), nameof(FullPrayerAbility.CanBeInterrupted))]
    //class t5
    //{
    //    public static bool Prefix(ref bool __result)
    //    {
    //        Main.Randomizer.LogWarning("Interrupt");
    //        __result = true;
    //        return false;
    //    }
    //}

    //[HarmonyPatch(typeof(FullPrayerAbility), nameof(FullPrayerAbility.CanBeExecutedConcurrentlyWith))]
    //class t6
    //{
    //    public static bool Prefix(ref bool __result)
    //    {
    //        Main.Randomizer.LogWarning("Concurrent");
    //        __result = true;
    //        return false;
    //    }
    //}

    //[HarmonyPatch(typeof(ChangeWeaponAbility), nameof(ChangeWeaponAbility.CanBeExecutedConcurrentlyWith))]
    //class t7
    //{
    //    public static bool Prefix(ref bool __result)
    //    {
    //        //Main.Randomizer.LogWarning("Concurrent change weapon");
    //        __result = true;
    //        return false;
    //    }
    //}

    [HarmonyPatch(typeof(ChangeWeaponAbility), nameof(ChangeWeaponAbility.OnStartExecution))]
    class t8
    {
        public static void Postfix() => Main.QualityOfLife.Log("Start change weapon");
    }
    [HarmonyPatch(typeof(ChangeWeaponAbility), nameof(ChangeWeaponAbility.OnFinalizeExecution))]
    class t9
    {
        public static void Postfix() => Main.QualityOfLife.Log("Finish change weapon");
    }
    [HarmonyPatch(typeof(FullPrayerAbility), nameof(FullPrayerAbility.OnStartExecution))]
    class t10
    {
        public static void Postfix() => Main.QualityOfLife.Log("Start prayer");
    }
    [HarmonyPatch(typeof(FullPrayerAbility), nameof(FullPrayerAbility.OnFinalizeExecution))]
    class t11
    {
        public static void Postfix() => Main.QualityOfLife.Log("Finish prayer");
    }

    //[HarmonyPatch(typeof(AbilityBlock), nameof(AbilityBlock.RequestBlock), new System.Type[] { })]
    //class t12
    //{
    //    public static void Postfix() => Main.Randomizer.Log("Request block: ---");
    //}
    //[HarmonyPatch(typeof(AbilityBlock), nameof(AbilityBlock.RequestBlock), new System.Type[] { typeof(CharacterAbility2D) })]
    //class t13
    //{
    //    public static void Postfix(CharacterAbility2D ability) => Main.Randomizer.Log("Request block: " + ability.GetIl2CppType().Name);
    //}
    //[HarmonyPatch(typeof(AbilityBlock), nameof(AbilityBlock.RequestBlock), new System.Type[] { typeof(IAbilityTypeRef) })]
    //class t14
    //{
    //    public static void Postfix(IAbilityTypeRef ability) => Main.Randomizer.Log("Request block: " + ability.name);
    //}

    //[HarmonyPatch(typeof(CharacterController2D), nameof(CharacterController2D.IsBlockedBy), typeof(Type), typeof(Type))]
    //class t15
    //{
    //    public static void Postfix(Type blocker, Type blocked, bool __result) => Main.QualityOfLife.Log($"{blocked.Name} is blocked by {blocker.Name}: {__result}");
    //}

    [HarmonyPatch(typeof(CharacterController2D), nameof(CharacterController2D.ActivateAbilityByType), typeof(IAbilityTypeRef))]
    class t16
    {
        public static void Postfix(IAbilityTypeRef abilityType) => Main.QualityOfLife.LogWarning("Activate prayer: " + abilityType.name);
    }

    //[HarmonyPatch(typeof(FullPrayerAbility), nameof(FullPrayerAbility.BlockAbility))]
    //class t17
    //{
    //    public static void Postfix(IAbilityTypeRef ability) => Main.Randomizer.LogWarning("Blocking: " + ability.name);
    //}

    //[HarmonyPatch(typeof(ChangeWeaponAbility), nameof(ChangeWeaponAbility.EnableAbility))]
    //class t18
    //{
    //    public static void Postfix(bool _enabled) => Main.Randomizer.LogWarning("Enabling: " + _enabled);
    //}
}
