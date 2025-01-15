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
    public static event Action OnSuccessfulHitEvent;
    public static event Action OnResetComboEvent;
    public static event Action OnCharacterManaChargeTriggerEvent;
    public static event Action<bool> OnCharacterGuardEvent;

    public static event Action OnUltimateTriggeredIn;
    public static event Action OnUltimateAttackEvent;
    public static event Action OnUltimateTriggeredOut;

    public static event Action OnBattleFocusInEvent;
    public static event Action OnBattleFocusOutEvent;

    public static event Action OnForwardStepEvent;
    public static event Action OnCharacterBoostInEvent;
    public static event Action OnCharacterBoostOutEvent;

    public static event Action OnAnimationJumpForceEvent;

    //Event Method

    #region Locomotion
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
    #endregion

    #region Attacks
    public static void TriggerAttack(){
        OnCharacterAttackTriggerEvent?.Invoke();
    }
    public static void TriggerSuccessfulHit(){
        OnSuccessfulHitEvent?.Invoke();
    }
    public static void TriggerResetCombo(){
        OnResetComboEvent?.Invoke();
    }
    public static void TriggerManaCharge(){
        OnCharacterManaChargeTriggerEvent?.Invoke();
    }
    #endregion

    #region Ultimate
    public static void TriggerUltimateIn(){
        OnUltimateTriggeredIn?.Invoke();
    }
    public static void TriggerUltimateAttack(){
        OnUltimateAttackEvent?.Invoke();
    }
    public static void TriggerUltimateOut(){
        OnUltimateTriggeredOut?.Invoke();
    }
    #endregion

    #region Guard
    public static void TriggerGuard(bool val){
        OnCharacterGuardEvent?.Invoke(val);
    }
    #endregion

    #region Camera
    public static void TriggerFocusCamIn(){
        OnBattleFocusInEvent?.Invoke();
    }
    public static void TriggerFocusCamOut(){
        OnBattleFocusOutEvent?.Invoke();
    }
    #endregion

    #region MISC
    public static void TriggerForwardStep(){
        OnForwardStepEvent?.Invoke();
    }
    public static void CharacterBoostIn(){
        OnCharacterBoostInEvent?.Invoke();
    }
    public static void CharacterBoostOut(){
        OnCharacterBoostOutEvent?.Invoke();
    }
    #endregion
}