using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : AnimatorUtil
{
    [SerializeField] private float _movementLerper = .5f;
    [SerializeField] AnimationCurve _rumbleFq;
    [SerializeField] private Animator _anim;

    private float _moveVal;
    private int _activeWeaponIndex;

    //Animator Variables
    private float _movement;

    private void Start() {
        GameEventSystem._current.OnCharacterIdleEvent += Idle;
        GameEventSystem._current.OnCharacterWalkEvent += Walk;
        GameEventSystem._current.OnCharacterRunEvent += Run;

        GameEventSystem._current.OnCharacterJumpEvent += Jump;
        GameEventSystem._current.OnCharacterSwitchWeapon += Switch;

        GameEventSystem._current.OnCharacterAimInEvent += AimIn;
        GameEventSystem._current.OnCharacterAimOutEvent += AimOut;
    }

    private void OnDisable() {
        GameEventSystem._current.OnCharacterIdleEvent -= Idle;
        GameEventSystem._current.OnCharacterWalkEvent -= Walk;
        GameEventSystem._current.OnCharacterRunEvent -= Run;

        GameEventSystem._current.OnCharacterJumpEvent -= Jump;
        GameEventSystem._current.OnCharacterSwitchWeapon -= Switch;

        GameEventSystem._current.OnCharacterAimInEvent -= AimIn;
        GameEventSystem._current.OnCharacterAimOutEvent -= AimOut;
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
    public void Switch(){
        AnimatorTrigger(_anim, "Switch Weapon", 0.5f);
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