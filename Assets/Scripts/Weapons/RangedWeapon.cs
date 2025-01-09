using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangedWeapon : WeaponScript{

    [Header("Ranged Weapon Properties")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _speed;

    public void Attack(){
        
    }

    private void Update() {

    }
}