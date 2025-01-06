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

    public event Action OnCharacterAimInEvent;
    public event Action OnCharacterAimOutEvent;

    public event Action OnCharacterBoostInEvent;
    public event Action OnCharacterBoostOutEvent;

    public event Action OnCharacterEnemyLockInEvent;
    public event Action OnCharacterEnemyLockOutEvent;
    public event Action OnCharacterEnemyLockCycleEvent;

    public event Action OnAnimationJumpForceEvent;

    //Event Method
    public void CharacterIdle(){
        if(OnCharacterIdleEvent != null){
            OnCharacterIdleEvent();
        }
    }
    public void CharacterWalk(){
        if(OnCharacterWalkEvent != null){
            OnCharacterWalkEvent();
        }
    }
    public void CharacterRun(){
        if(OnCharacterRunEvent != null){
            OnCharacterRunEvent();
        }
    }
    public void CharacterJump(){
        if(OnCharacterJumpEvent != null){
            OnCharacterJumpEvent();
        }
    }
    public void AnimationJump(){
        if(OnAnimationJumpForceEvent != null){
            OnAnimationJumpForceEvent();
        }
    }

    public void TriggerAttack(){
        if(OnCharacterAttackTriggerEvent != null){
            OnCharacterAttackTriggerEvent();
        }
    }

    public void CharacterAimIn(){
        CharacterEnemyLockOut();
        if(OnCharacterAimInEvent != null){
            OnCharacterAimInEvent();
        }
    }
    public void CharacterAimOut(){
        if(OnCharacterAimOutEvent != null){
            OnCharacterAimOutEvent();
        }
    }
    public void CharacterBoostIn(){
        if(OnCharacterBoostInEvent != null){
            OnCharacterBoostInEvent();
        }
    }
    public void CharacterBoostOut(){
        if(OnCharacterBoostOutEvent != null){
            OnCharacterBoostOutEvent();
        }
    }
    public void CharacterEnemyLockIn(){
        if(OnCharacterEnemyLockInEvent != null){
            OnCharacterEnemyLockInEvent();
        }
    }
    public void CharacterEnemyLockOut(){
        if(OnCharacterEnemyLockOutEvent != null){
            OnCharacterEnemyLockOutEvent();
        }
    }
    public void CharacterEnemyLockCycle(){
        if(OnCharacterEnemyLockCycleEvent != null){
            OnCharacterEnemyLockCycleEvent();
        }
    }
}