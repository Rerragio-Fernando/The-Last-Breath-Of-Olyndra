using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : AnimatorUtil
{
    public static PlayerAnimationScript _current;
    [SerializeField] private float _movementLerper = .5f;
    [SerializeField] AnimationCurve _rumbleFq;

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
        PlayerEventSystem._current.OnCharacterWalkEvent += Walk;
        PlayerEventSystem._current.OnCharacterRunEvent += Run;

        PlayerEventSystem._current.OnCharacterJumpEvent += Jump;

        PlayerEventSystem._current.OnCharacterAttackTriggerEvent += TriggerAttack;

        PlayerEventSystem._current.OnCharacterBasicAttack1Event += Attack1;
        PlayerEventSystem._current.OnCharacterBasicAttack2Event += Attack2;
        PlayerEventSystem._current.OnCharacterBasicAttack3Event += Attack3;

        PlayerEventSystem._current.OnCharacterAimInEvent += AimIn;
        PlayerEventSystem._current.OnCharacterAimOutEvent += AimOut;
    }

    private void OnDisable() {
        PlayerEventSystem._current.OnCharacterIdleEvent -= Idle;
        PlayerEventSystem._current.OnCharacterWalkEvent -= Walk;
        PlayerEventSystem._current.OnCharacterRunEvent -= Run;

        PlayerEventSystem._current.OnCharacterJumpEvent -= Jump;

        PlayerEventSystem._current.OnCharacterAttackTriggerEvent -= TriggerAttack;

        PlayerEventSystem._current.OnCharacterBasicAttack1Event -= Attack1;
        PlayerEventSystem._current.OnCharacterBasicAttack2Event -= Attack2;
        PlayerEventSystem._current.OnCharacterBasicAttack3Event -= Attack3;

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
    public void TriggerAttack(){
        _anim.SetBool("IsAttacking", true);
    }
    public void Attack1(){
        TriggerAttack();
        _anim.SetInteger("Attack Index", 1);
    }
    public void Attack2(){
        TriggerAttack();
        _anim.SetInteger("Attack Index", 2);
    }
    public void Attack3(){
        TriggerAttack();
        _anim.SetInteger("Attack Index", 3);
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

    public void SetGrounded(bool val){
        _anim.SetBool("Grounded", val);
    }

    //Animation Events
    public void NotAttacking(){
        _anim.SetBool("IsAttacking", false);
    }
    // Add an event to falsify the Is Attacking parameter
}