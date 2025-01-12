using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : AnimatorUtil
{
    [SerializeField] private float _movementLerper = .5f;
    // [SerializeField] AnimationCurve _rumbleFq;

    private Animator _anim;
    private float _moveVal;
    private int _activeWeaponIndex;

    private void Start() {
        _anim = GetComponent<Animator>();

        PlayerEventSystem.OnSpawnInEvent += Spawn;

        PlayerEventSystem.OnCharacterIdleEvent += Idle;
        PlayerEventSystem.OnCharacterRunEvent += Run;

        PlayerEventSystem.OnCharacterJumpEvent += Jump;
        PlayerEventSystem.OnCharacterGuardEvent += Guard;

        PlayerEventSystem.OnCharacterAttackTriggerEvent += TriggerAttack;
    }

    private void OnDisable() {
        PlayerEventSystem.OnCharacterIdleEvent -= Idle;
        PlayerEventSystem.OnCharacterRunEvent -= Run;

        PlayerEventSystem.OnCharacterJumpEvent -= Jump;
        PlayerEventSystem.OnCharacterGuardEvent -= Guard;

        PlayerEventSystem.OnCharacterAttackTriggerEvent -= TriggerAttack;
    }

    public void Spawn(){
        Debug.Log($"Spawned In");
    }

    public void Idle(){
        BlendTreeValue(_anim, "Movement", 0f, _movementLerper);
    }
    public void Run(){
        BlendTreeValue(_anim, "Movement", 1f, _movementLerper);
    }
    public void Jump(){
        AnimatorTrigger(_anim, "Jump", 0.5f);
    }
    public void TriggerAttack(){
        AnimatorTrigger(_anim, "Attack", 0.5f);
    }
    public void Guard(bool val){
        _anim.SetBool("Guarding", val);
    }
    public void UpdateCharacterDirection(Vector2 direction){
        BlendTreeValue(_anim, "FrontBack", direction.y, _movementLerper);
        BlendTreeValue(_anim, "LeftRight", direction.x, _movementLerper);
    }

    public void SetGrounded(bool val){
        _anim.SetBool("Grounded", val);
    }
}