using HarmonyLib;
using Il2CppLightbug.Kinematic2D.Core;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Attack;
using UnityEngine;

namespace BlasII.QualityOfLife
{
    internal class TyphoonTimer
    {
        private Vector3 _lastPosition = Vector3.zero;

        public void Update()
        {
            //Main.QualityOfLife.LogWarning("Speed: " + PlayerRB.velocity.y);
            // init = 404970459, spin = -144600212
            var anim = CoreCache.PlayerSpawn.PlayerInstance.GetComponentInChildren<Animator>();
            int animState = anim.GetCurrentAnimatorStateInfo(0).nameHash;
            //Main.QualityOfLife.LogWarning("Anim state: " + animState);

            if (Input.GetKey(KeyCode.O) || animState == -144600212)
            {
                Main.QualityOfLife.Log("Giving speed");
                Vector3 newPos = _lastPosition + Vector3.up * TYPHOON_FORCE * Time.deltaTime;
                Body.bodyTransform = new BodyTransform() { position = newPos };
            }

            _lastPosition = Body.bodyTransform.position;
        }

        private Rigidbody2D _playerRB;
        private Rigidbody2D PlayerRB
        {
            get
            {
                if (_playerRB == null)
                    _playerRB = CoreCache.PlayerSpawn.PlayerInstance.GetComponent<Rigidbody2D>();
                return _playerRB;
            }
        }

        private CharacterBody2DImpl _body;
        private CharacterBody2DImpl Body
        {
            get
            {
                if (_body == null)
                    _body = CoreCache.PlayerSpawn.PlayerInstance.GetComponent<CharacterBody2DImpl>();
                return _body;
            }
        }

        private const float TYPHOON_FORCE = 8f;
    }

    [HarmonyPatch(typeof(CenserChargedAttack), nameof(CenserChargedAttack.OnUpdate))]
    class t
    {
        public static void Prefix(ref float dt)
        {
            //dt = (float)1 / 15;
            //Main.QualityOfLife.LogWarning($"Charged attack ({dt} vs {Time.deltaTime})");
        }
    }

    [HarmonyPatch(typeof(BodyTransform), nameof(BodyTransform.Translate))]
    class t2
    {
        public static void Prefix(Vector3 deltaPosition)
        {
            Main.QualityOfLife.LogWarning("Target: " + deltaPosition);
        }
    }

    [HarmonyPatch(typeof(CenserChargedAttack), nameof(CenserChargedAttack.OnStartExecution))]
    class t3
    {
        public static void Postfix()
        {
            Main.QualityOfLife.Log("Start charge attack");
        }
    }

    [HarmonyPatch(typeof(CenserChargedAttack), nameof(CenserChargedAttack.EndChargingAttack))]
    class t4
    {
        public static void Postfix(bool charged)
        {
            Main.QualityOfLife.Log("Finish charge attack");
        }
    }
}
