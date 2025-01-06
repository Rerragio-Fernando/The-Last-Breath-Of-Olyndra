using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangedWeapon : WeaponScript{

    [Header("Ranged Weapon Properties")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _speed;

    private bool _aimIN;

    private void Start() {
        PlayerInputHandler.AimEvent += AimInput;
    }

    #region Input Functions
        private void AimInput(InputActionPhase phase){
            if(phase != InputActionPhase.Canceled)
                _aimIN = true;
            else    
                _aimIN = false;
        }
    #endregion

    public void Attack(){
        
    }

    private void Update() {
        Aim();
    }

    public void Aim(){
        if(_aimIN)
            PlayerCameraHandler._current.ActivateAimCam();
        else
            PlayerCameraHandler._current.ActivateMainCam();
    }
}