using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : AnimatorUtil
{
    public static PlayerAnimationScript _current;
    [SerializeField] private float _movementLerper = .5f;
    // [SerializeField] AnimationCurve _rumbleFq;

    private Animator _anim;
    private float _moveVal;
    private int _activeWeaponIndex;

    private void Awake() {
        if(_current == null)
            _current = this;
        else
            Destroy(this);
    }

    private void Start() {
        _anim = GetComponent<Animator>();

        PlayerEventSystem._current.OnCharacterIdleEvent += Idle;
        PlayerEventSystem._current.OnCharacterRunEvent += Run;

        PlayerEventSystem._current.OnCharacterJumpEvent += Jump;
        PlayerEventSystem._current.OnCharacterGuardEvent += Guard;

        PlayerEventSystem._current.OnCharacterAttackTriggerEvent += TriggerAttack;
    }

    private void OnDisable() {
        PlayerEventSystem._current.OnCharacterIdleEvent -= Idle;
        PlayerEventSystem._current.OnCharacterRunEvent -= Run;

        PlayerEventSystem._current.OnCharacterJumpEvent -= Jump;
        PlayerEventSystem._current.OnCharacterGuardEvent -= Guard;

        PlayerEventSystem._current.OnCharacterAttackTriggerEvent -= TriggerAttack;
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