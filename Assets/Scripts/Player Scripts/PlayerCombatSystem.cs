using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using UnityEngine;

public class PlayerCombatSystem : MonoBehaviour
{
    [SerializeField] private float _movementTimeDelay;
    [SerializeField] private float _manaChargeDuration;
    [SerializeField] private float _manaChargeRate;

    [Header("Weapon Colliders")]
    [SerializeField] private Collider _scytheCollider;
    [SerializeField] private Collider _daggerCollider;

    [Header("VFX")]
    [SerializeField] private VisualEffect[] _manaFXAssets;
    [SerializeField] private GameObject _manaChargeFXPrefab;
    [SerializeField] private Transform _manaChargeTrans;

    private float _nextMove = 0f;
    private float _nextManaCharge = 0f;
    private bool _isAttacking = false;
    private bool _manaActive = false;
    private GameObject _manaFX;
    public bool IsAttacking {
        get{return _isAttacking;}
    }
    public enum CombatState{
        Anticipation,
        Attacking,
        NotAttacking,
        Guarding
    }

    private CombatState _playerCombatState;

    private void Start() {
        NotAttacking();
        PlayerInputHandler.GuardEvent += GuardInput;
        PlayerInputHandler.BasicAttackEvent += BasicAttackInput;
        PlayerInputHandler.ManaChargeEvent += ManaChargeInput;
        PlayerInputHandler.AOEAttackEvent += AOEAttackInput;
    }

    #region Input Functions
        bool _basicAtkIN, _manaChargeIN, _aoeAtkIN, _guardIN;
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
        void ManaChargeInput(InputActionPhase phase){
            if(phase == InputActionPhase.Started)
                _manaChargeIN = true;
            else
                _manaChargeIN = false;
        }
        void AOEAttackInput(InputActionPhase phase){
            if(phase == InputActionPhase.Performed)
                _aoeAtkIN = true;
            else
                _aoeAtkIN = false;
        }
    #endregion
    
    private void Update() {
        ManaFunction();
        if(_playerCombatState == CombatState.NotAttacking){
            if(_basicAtkIN){
                PlayerEventSystem.TriggerAttack();
            }
        }

        PlayerEventSystem.TriggerGuard(_guardIN);
    }

    private void ManaFunction(){
        if(_manaChargeIN){
            if(Time.time > _nextManaCharge){
                _nextManaCharge = Time.time + 1/_manaChargeRate;
                StartCoroutine(ManaCharge());
            }
        }
    }

    #region Animation Events
        public void Anticipation(){
            _playerCombatState = CombatState.Anticipation;
            _isAttacking = true;
            _nextMove = Time.time + _movementTimeDelay;
        }

        public void Attacking(){
            _playerCombatState = CombatState.Attacking;
        }

        public void NotAttacking(){
            _playerCombatState = CombatState.NotAttacking;
        }

        public void EndOfAnimation(){
            if(Time.time >= _nextMove)
                _isAttacking = false;
        }

        public void EnableScytheCollider(){
            _scytheCollider.enabled = true;
        }
        public void DisableScytheCollider(){
            _scytheCollider.enabled = false;
        }

        public void EnableDaggerCollider(){
            _daggerCollider.enabled = true;
        }
        public void DisableDaggerCollider(){
            _daggerCollider.enabled = false;
        }

        public void AttackAManaVFX(){
            _manaFXAssets[0].Play();
        }
        public void AttackBManaVFX(){
            _manaFXAssets[1].Play();
        }
        public void AttackCManaVFX(){
            _manaFXAssets[2].Play();
        }
        public void AttackDManaVFX(){
            _manaFXAssets[3].Play();
        }
        public void AttackEManaVFX(){
            _manaFXAssets[4].Play();
        }
        public void AttackFManaVFX(){
            _manaFXAssets[5].Play();
        }

    #endregion

    IEnumerator ManaCharge(){
        _manaActive = true;
        _manaFX = Instantiate(_manaChargeFXPrefab, _manaChargeTrans);

        yield return new WaitForSeconds(_manaChargeDuration);

        _manaActive = false;
    }
}