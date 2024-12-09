using BlasII.ModdingAPI.Utils;
using Il2CppLightbug.Kinematic2D.Core;
using Il2CppTGK.Game;
using UnityEngine;

namespace BlasII.QualityOfLife.ConsistentTyphoon;

internal class CTModule : BaseModule
{
    private Vector3 _lastPosition = Vector3.zero;

    private ObjectCache<CharacterBody2DImpl> _playerBody;
    private ObjectCache<Animator> _playerAnimator;

    public override void OnStart()
    {
        // Requires full delegate syntax otherwise it crashes
        _playerBody = new ObjectCache<CharacterBody2DImpl>(() => CoreCache.PlayerSpawn.PlayerInstance.GetComponent<CharacterBody2DImpl>());
        _playerAnimator = new ObjectCache<Animator>(() => CoreCache.PlayerSpawn.PlayerInstance.GetComponentInChildren<Animator>());
    }

    public override void OnUpdate()
    {
        if (!Main.QualityOfLife.CurrentSettings.ConsistentTyphoon || CoreCache.PlayerSpawn.PlayerInstance == null)
            return;

        int currentAnimation = _playerAnimator.Value.GetCurrentAnimatorStateInfo(0).nameHash;

        // If in censer spin animation, override movement with typhoon force
        if (currentAnimation == TYPHOON_ANIM_HASH)
        {
            Vector3 newPos = _lastPosition + Vector3.up * TYPHOON_FORCE * Time.deltaTime;
            _playerBody.Value.bodyTransform = new BodyTransform() { position = newPos };
        }

        _lastPosition = _playerBody.Value.bodyTransform.position;
    }

    private const float TYPHOON_FORCE = 3.4f;
    private const int TYPHOON_ANIM_HASH = -1482913320;
}
