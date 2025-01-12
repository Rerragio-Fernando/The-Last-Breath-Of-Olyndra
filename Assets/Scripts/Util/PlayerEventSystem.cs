using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventSystem : GameCharacterEventSystem
{
    //Event Declaration
    public static event Action OnCharacterIdleEvent;
    public static event Action OnCharacterWalkEvent;
    public static event Action OnCharacterRunEvent;
    public static event Action OnCharacterJumpEvent;

    public static event Action OnCharacterAttackTriggerEvent;
    public static event Action OnCharacterManaChargeTriggerEvent;
    public static event Action<bool> OnCharacterGuardEvent;

    public static event Action OnCharacterBoostInEvent;
    public static event Action OnCharacterBoostOutEvent;

    public static event Action OnAnimationJumpForceEvent;

    //Event Method
    public static void CharacterIdle(){
        OnCharacterIdleEvent?.Invoke();
    }
    public static void CharacterWalk(){
        OnCharacterWalkEvent?.Invoke();
    }
    public static void CharacterRun(){
        OnCharacterRunEvent?.Invoke();
    }
    public static void CharacterJump(){
        OnCharacterJumpEvent?.Invoke();
    }
    public static void AnimationJump(){
        OnAnimationJumpForceEvent?.Invoke();
    }

    public static void TriggerAttack(){
        OnCharacterAttackTriggerEvent?.Invoke();
    }
    public static void TriggerManaCharge(){
        OnCharacterManaChargeTriggerEvent?.Invoke();
    }

    public static void TriggerGuard(bool val){
        OnCharacterGuardEvent?.Invoke(val);
    }
    public static void CharacterBoostIn(){
        OnCharacterBoostInEvent?.Invoke();
    }
    public static void CharacterBoostOut(){
        OnCharacterBoostOutEvent?.Invoke();
    }
}