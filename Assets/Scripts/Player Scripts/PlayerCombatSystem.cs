using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerCombatSystem : MonoBehaviour
{
    private enum CombatStates{
        Anticipation,
        Attacking,
        NotAttacking
    }

    private CombatStates _playerCombatState;

    private void Start() {
        EndOfAttack();
        PlayerInputHandler.BasicAttackEvent += BasicAttackInput;
        PlayerInputHandler.StrongAttackEvent += StrongAttackInput;
        PlayerInputHandler.AOEAttackEvent += AOEAttackInput;
    }

    #region Input Functions
        bool _basicAtkIN, _strongAtkIN, _aoeAtkIN;
        void BasicAttackInput(InputActionPhase phase){
            if(phase == InputActionPhase.Started)
                _basicAtkIN = true;
            else
                _basicAtkIN = false;
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
    }

    #region Animation Events
        public void Anticipation(){
            _playerCombatState = CombatStates.Anticipation;
        }

        public void Attacking(){
            _playerCombatState = CombatStates.Attacking;
            PlayerEventSystem._current.CharacterBoostIn();
        }

        public void EndOfAttack(){
            _playerCombatState = CombatStates.NotAttacking;
        }
    #endregion
}