using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatScript : MonoBehaviour
{   
    [Header("Attack Properties")]
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _lockBoostRange;
    [SerializeField] private float _boostRate;
    [SerializeField] private GameObject _boostFX;
    [SerializeField] private GameObject _enemyHitFX;

    [Header("Basic Attack Properties")]
    [SerializeField] private float _basicAttackDamage;
    [SerializeField] private float _basicAttackForce;
    [SerializeField] private float _basicAttackRange;
    [SerializeField] private GameObject []_basicAtkFX;

    [Header("AOE Attack Properties")]
    [SerializeField] private float _aoeAttackDamage;
    [SerializeField] private float _aoeAttackForce;
    [SerializeField] private float _aoeAttackRange;
    [SerializeField] private GameObject _aoeAtkFX;

    [Header("Strong Attack Properties")]
    [SerializeField] private float _strongAttackDamage;
    [SerializeField] private float _strongAttackRange;
    [SerializeField] private GameObject _strongAtkFX;

    private float _nextBoost = 0f;
    public int _basicIndx = 1;
    private RaycastHit _hit;
    private bool _boostTriggered = false;

    private void Start() {
        PlayerEventSystem._current.OnCharacterBoostInEvent += BoostFx;
    }
    
    private void Update() {

        // PlayerBoost();

        if(!_isAttacking && _isReadyToAttack){
            if(PlayerInputHandler.IN_Attack.attack){
                _isAttacking = true;
                _isReadyToAttack = false;
                switch(PlayerInputHandler.IN_Attack.attackType){
                case 1:
                    BasicAttackLogic();
                    break;
                default:
                    _basicIndx = 1;
                    break;
                }
            }
            else
                _basicIndx = 1;
        }
    }

    void PlayerBoost(){
        if(PlayerInputHandler.IN_LockInput && PlayerInputHandler.IN_MoveInputRaw.normalized.magnitude <= 0f){
            if(PlayerInputHandler.IN_Attack.attack && PlayerInputHandler.IN_Attack.attackType == 1){
                float l_distance = Vector3.Distance(transform.position, PlayerLockOnScript.currentFocusedEnemy.transform.position);
                if(l_distance > _basicAttackRange && l_distance < _lockBoostRange){
                    if(Time.time > _nextBoost){
                        PlayerEventSystem._current.CharacterBoostIn();
                        _nextBoost = Time.time + 1/_boostRate;
                    }
                    PlayerEventSystem._current.CharacterBoostIn();
                }
                if(l_distance <= _basicAttackRange){
                    PlayerEventSystem._current.CharacterBoostOut();
                    _boostTriggered = false;
                }
            }
        }
    }

    void BasicAttackLogic(){
        switch(_basicIndx){
            case 1:
                PlayerEventSystem._current.CharacterAttack1();
                _basicIndx++;
                break;
            case 2:
                PlayerEventSystem._current.CharacterAttack2();
                _basicIndx++;
                break;
            case 3:
                PlayerEventSystem._current.CharacterAttack3();
                _basicIndx = 1;
                break;
        }
    }

    void StrongAttackLogic(){

    }

    void AOEAttackLogic(){

    }

    //Attack Logic
    // void BasicAttackLogic(){
    //     Vector3 l_rayStartPos = transform.position + new Vector3(0f, 1.5f, 0f);
    //     if(Physics.Raycast(l_rayStartPos, transform.TransformDirection(Vector3.forward), out _hit, _basicAttackRange, _enemyLayer)){
    //         Debug.DrawRay(l_rayStartPos, transform.TransformDirection(Vector3.forward) * _hit.distance, Color.red);
    //         var hitFx = Instantiate(_enemyHitFX, _hit.point, Quaternion.identity);
    //         _hit.transform.gameObject.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * _basicAttackForce, ForceMode.Impulse);
    //     }
    // }

    // void AOEAttackLogic(){
    //     Collider[] hitColliders = Physics.OverlapSphere(transform.position, _aoeAttackRange, _enemyLayer);
    //     if(hitColliders != null){
    //         foreach (var hitCollider in hitColliders)
    //         {
    //             hitCollider.gameObject.GetComponent<Rigidbody>().AddForce(
    //                 (hitCollider.gameObject.transform.position - transform.position) * _aoeAttackForce
    //                 , ForceMode.Impulse);
    //         }
    //     }
    // }

    void BoostFx(){
        if(_boostTriggered)
            return;
        
        _boostTriggered = true;
        var l_fx = Instantiate(_boostFX, transform.position, Quaternion.identity);
    }

    //Attack Variables
    #region Animation Events
        [Header("Animation Event Properties")]
        [SerializeField] private bool _isReadyToAttack = true;
        [SerializeField] private bool _isAttacking = false;
        public bool GetIsAttacking(){
            return _isAttacking;
        }
        public void ReadyToAttack(){
            Debug.Log($"Triggered ReadyToAttack");
            _isReadyToAttack = true;
            _isAttacking = false;
        }
        public void Hit(){
            var hitFx = Instantiate(_basicAtkFX[_basicIndx - 1], _hit.point, Quaternion.identity);
        }
        public void ResetBasicAttackIndex(){
            _basicIndx = 1;
        }
    #endregion
}