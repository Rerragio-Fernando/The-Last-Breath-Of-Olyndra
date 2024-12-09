using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : WeaponScript{

    [Header("Ranged Weapon Properties")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _speed;

    public void Attack(){
        
    }

    private void Update() {
        Aim();
    }

    public void Aim(){
        if(PlayerInputHandler.IN_Aim > 0f)
            PlayerCameraHandler._current.ActivateAimCam();
        else
            PlayerCameraHandler._current.ActivateMainCam();
    }
}