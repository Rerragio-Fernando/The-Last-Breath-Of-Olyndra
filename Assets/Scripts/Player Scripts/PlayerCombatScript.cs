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
    [SerializeField] private float _basicAtkRate;
    [SerializeField] private GameObject []_basicAtkFX;

    [Header("AOE Attack Properties")]
    [SerializeField] private float _aoeAttackDamage;
    [SerializeField] private float _aoeAttackForce;
    [SerializeField] private float _aoeAttackRange;
    [SerializeField] private float _aoeAtkRate;
    [SerializeField] private GameObject _aoeAtkFX;

    [Header("Strong Attack Properties")]
    [SerializeField] private float _strongAttackDamage;
    [SerializeField] private float _strongAttackRange;
    [SerializeField] private float _strongAtkRate;
    [SerializeField] private GameObject _strongAtkFX;

    private float _nextAtk = 0f;
    private float _nextBoost = 0f;
    private int _basicIndx = 0;
    private RaycastHit _hit;
    private bool _boostTriggered = false;
    private void Start() {
        PlayerEventSystem._current.OnCharacterBoostInEvent += BoostFx;
    }
    
    private void Update() {
        // Debug.Log($"Attacking: " + _attackIN.attack);
        if(PlayerInputHandler.IN_LockInput && PlayerInputHandler.IN_MoveInputRaw.normalized.magnitude <= 0f){
            if(PlayerInputHandler.IN_Attack.attack && PlayerInputHandler.IN_Attack.attackType == 1){
                float l_distance = Vector3.Distance(transform.position, PlayerLockOnScript.currentFocusedEnemy.transform.position);
                if(l_distance > _basicAttackRange && l_distance < _lockBoostRange){
                    // if(Time.time > _nextBoost){
                    //     PlayerEventSystem._current.CharacterBoostIn();
                    //     _nextBoost = Time.time + 1/_boostRate;
                    // }
                    PlayerEventSystem._current.CharacterBoostIn();
                }
                if(l_distance <= _basicAttackRange){
                    PlayerEventSystem._current.CharacterBoostOut();
                    _boostTriggered = false;
                }
            }
        }

        if(PlayerInputHandler.IN_Attack.attack){
            if(Time.time >= _nextAtk){
                switch(PlayerInputHandler.IN_Attack.attackType){
                    case 1:
                        BasicAttack();
                        return;
                    case 2:
                        StrongAttack();
                        return;
                    case 3:
                        AOEAttack();
                        return;
                }
            }
        }
    }

    //Attack Functions
    void BasicAttack(){
        BasicAttackLogic();
        LoopThroughBasicAttackFX();
        IncrementNextAtk(_basicAtkRate);
    }
    void StrongAttack(){
        IncrementNextAtk(_strongAtkRate);
        SpawnFX(_strongAtkFX);
        _basicIndx = 0;
    }
    void AOEAttack(){
        IncrementNextAtk(_aoeAtkRate);
        SpawnFX(_aoeAtkFX);
        AOEAttackLogic();
        // PlayerAnimationScript._current.Attack("AOE");
        _basicIndx = 0;
    }

    void LoopThroughBasicAttackFX(){
        SpawnFX(_basicAtkFX[_basicIndx]);

        _basicIndx++;
        if(_basicIndx >= _basicAtkFX.Length)
            _basicIndx = 0;

        // PlayerAnimationScript._current.Attack("Melee " + (_basicIndx + 1));
    }

    void IncrementNextAtk(float val){
        _nextAtk = Time.time + 1/val;
    }

    void SpawnFX(GameObject fx){
        var l_fx = Instantiate(fx, transform);
    }

    //Attack Logic
    void BasicAttackLogic(){
        Vector3 l_rayStartPos = transform.position + new Vector3(0f, 1.5f, 0f);
        if(Physics.Raycast(l_rayStartPos, transform.TransformDirection(Vector3.forward), out _hit, _basicAttackRange, _enemyLayer)){
            Debug.DrawRay(l_rayStartPos, transform.TransformDirection(Vector3.forward) * _hit.distance, Color.red);
            var hitFx = Instantiate(_enemyHitFX, _hit.point, Quaternion.identity);
            PlayerEventSystem._current.CharacterAttack();
            _hit.transform.gameObject.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * _basicAttackForce, ForceMode.Impulse);
        }
    }

    void AOEAttackLogic(){
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _aoeAttackRange, _enemyLayer);
        if(hitColliders != null){
            foreach (var hitCollider in hitColliders)
            {
                hitCollider.gameObject.GetComponent<Rigidbody>().AddForce(
                    (hitCollider.gameObject.transform.position - transform.position) * _aoeAttackForce
                    , ForceMode.Impulse);
            }
        }
    }

    void BoostFx(){
        if(_boostTriggered)
            return;
        
        _boostTriggered = true;
        var l_fx = Instantiate(_boostFX, transform.position, Quaternion.identity);
    }
}