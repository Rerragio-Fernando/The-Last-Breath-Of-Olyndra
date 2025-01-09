using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventSystem : MonoBehaviour
{
    public static PlayerEventSystem _current;

    private void Awake(){
        _current = this;
    } 

    //Event Declaration
    public event Action OnCharacterIdleEvent;
    public event Action OnCharacterWalkEvent;
    public event Action OnCharacterRunEvent;
    public event Action OnCharacterJumpEvent;

    public event Action OnCharacterAttackTriggerEvent;
    public event Action<bool> OnCharacterGuardEvent;

    public event Action OnCharacterBoostInEvent;
    public event Action OnCharacterBoostOutEvent;

    public event Action OnAnimationJumpForceEvent;

    //Event Method
    public void CharacterIdle(){
        OnCharacterIdleEvent?.Invoke();
    }
    public void CharacterWalk(){
        OnCharacterWalkEvent?.Invoke();
    }
    public void CharacterRun(){
        OnCharacterRunEvent?.Invoke();
    }
    public void CharacterJump(){
        OnCharacterJumpEvent?.Invoke();
    }
    public void AnimationJump(){
        OnAnimationJumpForceEvent?.Invoke();
    }

    public void TriggerAttack(){
        OnCharacterAttackTriggerEvent?.Invoke();
    }

    public void TriggerGuard(bool val){
        OnCharacterGuardEvent?.Invoke(val);
    }
    public void CharacterBoostIn(){
        OnCharacterBoostInEvent?.Invoke();
    }
    public void CharacterBoostOut(){
        OnCharacterBoostOutEvent?.Invoke();
    }
}