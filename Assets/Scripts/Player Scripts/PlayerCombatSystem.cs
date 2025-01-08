using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerCombatSystem : MonoBehaviour
{
    public enum CombatStates{
        Anticipation,
        Attacking,
        NotAttacking,
        Guarding
    }

    private CombatStates _playerCombatState;
    public CombatStates PlayerCombatState{
        get { return _playerCombatState; }   // get method
        set { _playerCombatState = value; }  // set method
    }

    private void Start() {
        EndOfAttack();
        PlayerInputHandler.GuardEvent += GuardInput;
        PlayerInputHandler.BasicAttackEvent += BasicAttackInput;
        PlayerInputHandler.StrongAttackEvent += StrongAttackInput;
        PlayerInputHandler.AOEAttackEvent += AOEAttackInput;
    }

    #region Input Functions
        bool _basicAtkIN, _strongAtkIN, _aoeAtkIN, _guardIN;
        void BasicAttackInput(InputActionPhase phase){
            if(phase == InputActionPhase.Started)
                _basicAtkIN = true;
            else
                _basicAtkIN = false;
        }
        void GuardInput(InputActionPhase phase){
            if(phase == InputActionPhase.Performed)
                _guardIN = true;
            else
                _guardIN = false;
        }
        void StrongAttackInput(InputActionPhase phase){
            if(phase == InputActionPhase.Performed)
                _strongAtkIN = true;
            else
                _strongAtkIN = false;
        }
        void AOEAttackInput(InputActionPhase phase){
            if(phase == InputActionPhase.Performed)
                _aoeAtkIN = true;
            else
                _aoeAtkIN = false;
        }
    #endregion
    
    private void Update() {
        if(_playerCombatState == CombatStates.NotAttacking){
            if(_basicAtkIN)
                PlayerEventSystem._current.TriggerAttack();
        }

        PlayerEventSystem._current.TriggerGuard(_guardIN);
    }

    #region Animation Events
        public void Anticipation(){
            _playerCombatState = CombatStates.Anticipation;
        }

        public void Attacking(){
            _playerCombatState = CombatStates.Attacking;
        }

        public void EndOfAttack(){
            _playerCombatState = CombatStates.NotAttacking;
        }
    #endregion
}