using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using UnityEngine;
using UnityEngine.UI;


public class PlayerCombatSystem : MonoBehaviour
{
    [SerializeField] private float _movementTimeDelay;
    [SerializeField] private float _manaChargeDuration;
    [SerializeField] private float _manaChargeRate;

    [Header("Attack Properties")]
    [SerializeField] private Transform _raycastTrans;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float _playerNormalAtkRange;
    [SerializeField] private float _playerManaAtkRange;

    [Header("Ultimate Properties")]
    [SerializeField] private GameObject _ultimatePrefab;
    [SerializeField] private float _playerUltimateHoldDownTime;
    [SerializeField] private float _ultimateDuration;

    [Header("VFX")]
    [SerializeField] private AttackProfile[] _attackProfile;
    [SerializeField] private GameObject _hitFX;
    [SerializeField] private GameObject _manaChargeFXPrefab;
    [SerializeField] private GameObject _ultLoadPrefab;
    [SerializeField] private Transform _manaChargeTrans;
    [SerializeField] private Transform _atkFXParent;

    private bool _isAttacking = false;
    private bool _manaActive = false;
    private bool _enteringUltState = false;
    private bool _ulted = false;
    private int _minManaAmount;//Minimum amount of mana needed to make a charge
    private GameObject _manaFX;
    private GameObject _ultimateLoadFX;
    private GameObject _enemyObj;
    private ManaHandler _manaHandler;
    private PlayerComboScript _comboScript;
    private RaycastHit _hit;

    //Timer Variables
    private float _nextMove = 0f;
    private float _nextManaRegen = 0f;
    private float _nextManaCharge = 0f;
    private float _manaChargeTimer = 0f;
    private float _ultTimeHoldEnd = 0f;


    public bool IsAttacking {
        get{return _isAttacking;}
    }

    private CombatState _playerCombatState;

    private void Start() {
        _enemyObj = GameObject.FindWithTag("Enemy");
        _manaHandler = GetComponent<ManaHandler>();
        _comboScript = GetComponent<PlayerComboScript>();

        FindMinimumSufficientMana();
        NotAttacking();

        PlayerInputHandler.GuardEvent += GuardInput;
        PlayerInputHandler.BasicAttackEvent += BasicAttackInput;
        PlayerInputHandler.ManaChargeEvent += ManaChargeInput;
        PlayerInputHandler.UltimateEvent += UltimateInput;
    }

    void FindMinimumSufficientMana(){
        //Used to find the minimum amount of mana required to make a charge
        _minManaAmount = _manaHandler.PlayerMaxMana;
        foreach (AttackProfile item in _attackProfile)
        {
            if(item._manaCost < _minManaAmount)
                _minManaAmount = item._manaCost;
        }
    }

    #region Input Functions
        bool _basicAtkIN, _manaChargeIN, _guardIN;
        public bool _ultimateHoldIN, _ultimateStartedIN;
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
        void UltimateInput(InputActionPhase phase){
            if(phase == InputActionPhase.Started)
                _ultimateStartedIN = true;
            else
                _ultimateStartedIN = false;

            if(phase == InputActionPhase.Performed)
                _ultimateHoldIN = true;
            else
                _ultimateHoldIN = false;
        }
    #endregion
    
    private void Update(){
        //If Ulti button held down for "n" seconds then perform ult
        // The player can only ult within this time frame
        if(_ultimateStartedIN && !_enteringUltState){
            _enteringUltState = true;
            _ulted = false;
            _ultTimeHoldEnd = Time.time + _playerUltimateHoldDownTime;
            _ultimateLoadFX = Instantiate(_ultLoadPrefab, transform);
        }

        if(_ultimateHoldIN && _playerCombatState != CombatState.Ultimate){
            if((Time.time >= _ultTimeHoldEnd)){
                //If player keeps holding down the button till after x amount of seconds then player enters ultimate state
                _playerCombatState = CombatState.Ultimate;
                PlayerEventSystem.TriggerUltimateIn();                                                      //EVENT TRIGGERED
            }
        }

        //Checks if the state is in "Ultimate"
        if(_playerCombatState != CombatState.Ultimate){
            ManaChargeFunction();

            //This deactivates mana-charge after "_manaChargeTimer"
            if(_manaActive){
                if(Time.time > _manaChargeTimer)// if time exceeds the charge timer then deactivate mana
                    _manaActive = false;
            }

            if(_playerCombatState == CombatState.NotAttacking){
                if(_basicAtkIN){
                    PlayerEventSystem.TriggerAttack();                                                      //EVENT TRIGGERED
                }
            }
            PlayerEventSystem.TriggerGuard(_guardIN);                                                     //EVENT TRIGGERED
        }
        else{
            _isAttacking = true;
            UltimateFunctionality();
        }
    }

    private void ManaChargeFunction(){
        if(_manaChargeIN){
            if(Time.time > _nextManaCharge && (_manaHandler.PlayerMana >= _minManaAmount)){
                _nextManaCharge = Time.time + 1/_manaChargeRate;
                _manaChargeTimer = Time.time + _manaChargeDuration;
                _manaActive = true;
                _manaFX = Instantiate(_manaChargeFXPrefab, _manaChargeTrans);
            }
        }
    }

    void UltimateFunctionality(){
        if(_basicAtkIN && !_ulted){
            PlayerEventSystem.TriggerUltimateAttack();                                                      //EVENT TRIGGERED
            Destroy(_ultimateLoadFX);
            _ulted = true;
            _enteringUltState = false;
        }
    }

    private void AttackLogic(int indx){
        bool l_manaAttack = _manaActive && (_attackProfile[indx]._manaCost <= _manaHandler.PlayerMana);// if mana is active AND if there is sufficient mana

        if(l_manaAttack)
            _manaHandler.UseMana(_attackProfile[indx]._manaCost);

        int l_damage = l_manaAttack ? _attackProfile[indx]._manaAttackDamage : _attackProfile[indx]._normalAttackDamage;
        GameObject l_atkFXPrefab = l_manaAttack ? _attackProfile[indx]._manaAttackFX : _attackProfile[indx]._normalAttackFX;
        float range = l_manaAttack ? _playerManaAtkRange : _playerNormalAtkRange;
        float force = l_manaAttack ? _attackProfile[indx]._manaAttackForce : _attackProfile[indx]._normalAttackForce;
        
        CheckHit(l_damage, range, force);
        GameObject l_fx = Instantiate(l_atkFXPrefab, _atkFXParent.position, Quaternion.LookRotation(transform.forward));
        Destroy(l_fx, 1f);
    }

    private void CheckHit(int damage, float range, float force){
        if (Physics.Raycast(_raycastTrans.position, transform.forward, out _hit, range, layerMask)){
            var l_hitFx = Instantiate(_hitFX, _hit.point, Quaternion.LookRotation(_hit.normal));//Spawn Hit Effect
            Destroy(l_hitFx, 1f);

            GameObject l_hitObj = _hit.transform.gameObject;
            
            EnemyDamageScript l_ds = l_hitObj.GetComponent<EnemyDamageScript>();
            if(l_ds != null){
                PlayerEventSystem.TriggerSuccessfulHit();                                                       //EVENT TRIGGERED
                _manaHandler.GainMana(l_ds.TakeDamage(damage, _comboScript.Combo));
            }

            Rigidbody l_rb = l_hitObj.GetComponent<Rigidbody>();
            if(l_rb != null){
                l_rb.AddForce(force * transform.forward, ForceMode.Impulse);
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
            PlayerEventSystem.TriggerForwardStep();                                                     //EVENT TRIGGERED
        }

        public void NotAttacking(){
            _playerCombatState = CombatState.NotAttacking;
        }

        public void EndOfAnimation(){
            if(Time.time >= _nextMove)
                _isAttacking = false;
        }

        public void AttackAManaVFX(){
            AttackLogic(0);
        }
        public void AttackBManaVFX(){
            AttackLogic(1);
        }
        public void AttackCManaVFX(){
            AttackLogic(2);
        }
        public void AttackDManaVFX(){
            AttackLogic(3);
        }
        public void AttackEManaVFX(){
            AttackLogic(4);
        }
        public void AttackFManaVFX(){
            AttackLogic(5);
        }

        public void UltiAttackVFX(){
            GameObject l_ultFX = Instantiate(_ultimatePrefab, _atkFXParent.position, Quaternion.LookRotation(transform.forward));
            Destroy(l_ultFX, _ultimateDuration);
        }
        
    #endregion
}

public enum CombatState{
    Anticipation,
    Attacking,
    NotAttacking,
    Guarding,
    Ultimate
}