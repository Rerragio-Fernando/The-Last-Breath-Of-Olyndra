using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : AnimatorUtil
{
    [SerializeField] private float _movementLerper = .5f;
    [SerializeField] AnimationCurve _rumbleFq;

    private Animator _anim;
    private float _moveVal;
    private int _activeWeaponIndex;

    private void Start() {
        _anim = GetComponent<Animator>();

        PlayerEventSystem._current.OnCharacterIdleEvent += Idle;
        PlayerEventSystem._current.OnCharacterWalkEvent += Walk;
        PlayerEventSystem._current.OnCharacterRunEvent += Run;

        PlayerEventSystem._current.OnCharacterJumpEvent += Jump;

        PlayerEventSystem._current.OnCharacterAimInEvent += AimIn;
        PlayerEventSystem._current.OnCharacterAimOutEvent += AimOut;
    }

    private void OnDisable() {
        PlayerEventSystem._current.OnCharacterIdleEvent -= Idle;
        PlayerEventSystem._current.OnCharacterWalkEvent -= Walk;
        PlayerEventSystem._current.OnCharacterRunEvent -= Run;

        PlayerEventSystem._current.OnCharacterJumpEvent -= Jump;

        PlayerEventSystem._current.OnCharacterAimInEvent -= AimIn;
        PlayerEventSystem._current.OnCharacterAimOutEvent -= AimOut;
    }

    public void Idle(){
        BlendTreeValue(_anim, "Movement", 0f, _movementLerper);
    }
    public void Walk(){
        BlendTreeValue(_anim, "Movement", 1f, _movementLerper);
    }
    public void Run(){
        BlendTreeValue(_anim, "Movement", 2f, _movementLerper);
    }
    public void Jump(){
        AnimatorTrigger(_anim, "Jump", 0.5f);
    }
    public void Attack(string val){
        _anim.Play(val, 1, 0f);
    }
    public void AimIn(){
        _anim.SetBool("Aiming", true);
    }
    public void AimOut(){
        _anim.SetBool("Aiming", false);
    }
    public void UpdateCharacterDirection(Vector2 direction){
        BlendTreeValue(_anim, "FrontBack", direction.y, _movementLerper);
        BlendTreeValue(_anim, "LeftRight", direction.x, _movementLerper);
    }
    public void SetActiveWeaponIndex(int val){
        _activeWeaponIndex = val;
    }

    public void SetGrounded(bool val){
        _anim.SetBool("Grounded", val);
    }
}