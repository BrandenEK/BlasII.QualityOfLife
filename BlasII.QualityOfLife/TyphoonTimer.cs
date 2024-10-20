using Il2CppLightbug.Kinematic2D.Core;
using Il2CppTGK.Game;
using UnityEngine;

namespace BlasII.QualityOfLife;

internal class TyphoonTimer
{
    private Vector3 _lastPosition = Vector3.zero;

    public void Update()
    {
        if (!QualityOfLife.IsModuleActive("Consistent_Typhoon") || CoreCache.PlayerSpawn.PlayerInstance == null)
            return;

        int currentAnimation = PlayerAnim.GetCurrentAnimatorStateInfo(0).nameHash;

        // If in censer spin animation, override movement with typhoon force
        if (currentAnimation == -144600212)
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
}
