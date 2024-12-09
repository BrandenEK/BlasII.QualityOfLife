using BlasII.ModdingAPI;
using Il2CppLightbug.Kinematic2D.Core;
using Il2CppTGK.Game;
using UnityEngine;

namespace BlasII.QualityOfLife.ConsistentTyphoon;

internal class CTModule : BaseModule
{
    private Vector3 _lastPosition = Vector3.zero;

    public override void OnUpdate()
    {
        if (!Main.QualityOfLife.CurrentSettings.ConsistentTyphoon || CoreCache.PlayerSpawn.PlayerInstance == null)
            return;

        int currentAnimation = PlayerAnim.GetCurrentAnimatorStateInfo(0).nameHash;

        // If in censer spin animation, override movement with typhoon force
        if (currentAnimation == TYPHOON_ANIM_HASH)
        {
            Vector3 newPos = _lastPosition + Vector3.up * TYPHOON_FORCE * Time.deltaTime;
            PlayerBody.bodyTransform = new BodyTransform() { position = newPos };
        }

        _lastPosition = PlayerBody.bodyTransform.position;
    }

    private CharacterBody2DImpl _playerBody;
    private CharacterBody2DImpl PlayerBody
    {
        get
        {
            if (_playerBody == null)
                _playerBody = CoreCache.PlayerSpawn.PlayerInstance.GetComponent<CharacterBody2DImpl>();
            return _playerBody;
        }
    }

    private Animator _playerAnim;
    private Animator PlayerAnim
    {
        get
        {
            if (_playerAnim == null)
                _playerAnim = CoreCache.PlayerSpawn.PlayerInstance.GetComponentInChildren<Animator>();
            return _playerAnim;
        }
    }

    private const float TYPHOON_FORCE = 3.4f;
    private const int TYPHOON_ANIM_HASH = -1482913320;
}
