using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventSystem : MonoBehaviour
{
    public static GameEventSystem _current;

    private void Awake(){
        _current = this;
    } 

    //Event Declaration
    public event Action OnCharacterIdleEvent;
    public event Action OnCharacterWalkEvent;
    public event Action OnCharacterRunEvent;
    public event Action OnCharacterJumpEvent;
    public event Action OnCharacterAttackEvent;
    public event Action OnCharacterAimInEvent;
    public event Action OnCharacterAimOutEvent;
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
    public void CharacterAttack(){
        if(OnCharacterAttackEvent != null){
            OnCharacterAttackEvent();
        }
    }
    public void CharacterAimIn(){
        if(OnCharacterAimInEvent != null){
            OnCharacterAimInEvent();
        }
    }
    public void CharacterAimOut(){
        if(OnCharacterAimOutEvent != null){
            OnCharacterAimOutEvent();
        }
    }
}