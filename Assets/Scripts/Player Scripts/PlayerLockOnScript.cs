using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLockOnScript : MonoBehaviour
{
    [SerializeField] private float _enemyDetectionRange;
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private GameObject _lockFX;

    [SerializeField] private GameObject []_enemyObjects;
    public static GameObject currentFocusedEnemy;
    private int _currentIndex = 0;
    [SerializeField] private GameObject _activeLockFx = null;

    private void Start() {
        PlayerEventSystem._current.OnCharacterEnemyLockInEvent += LockedIn;
        PlayerEventSystem._current.OnCharacterEnemyLockCycleEvent += LockCycle;
        PlayerEventSystem._current.OnCharacterEnemyLockOutEvent += LockOut;
    }
    private void Update() {
        Debug.Log($"Locked In: " + PlayerInputHandler.IN_LockInput);
    }
    void LockCycle(){
        if(PlayerInputHandler.IN_LockInput){
            _currentIndex++;
            if(_currentIndex >= _enemyObjects.Length)
                _currentIndex = 0;
            
            currentFocusedEnemy = _enemyObjects[_currentIndex];
            LockOntoTarget();
        }
    }

    void LockOntoTarget(){
        Destroy(_activeLockFx);
        _activeLockFx = Instantiate(_lockFX, currentFocusedEnemy.transform);
    }

    void LockOutFromTarget(){
        Destroy(_activeLockFx);
    }

    void LockedIn(){
        AllocateNearbyEnemiesToArray();
        if(_enemyObjects != null){
            currentFocusedEnemy = _enemyObjects[0];
            LockOntoTarget();
        }
        else{
            currentFocusedEnemy = null;
            LockOutFromTarget();
        }
    }
    void AllocateNearbyEnemiesToArray(){
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _enemyDetectionRange, _enemyLayerMask);
        if(hitColliders != null){
            _currentIndex = 0;
            _enemyObjects = new GameObject[hitColliders.Length];
            int x = 0;
            foreach (var hitCollider in hitColliders)
            {
                _enemyObjects[x] = hitCollider.gameObject;
                x++;
            }
        }
        else{
            PlayerInputHandler.IN_LockInput = false;
            _enemyObjects = null;
        }
    }

    void LockOut(){
        PlayerInputHandler.IN_LockInput = false;
        _enemyObjects = null;
        currentFocusedEnemy = null;
        LockOutFromTarget();
    }
}