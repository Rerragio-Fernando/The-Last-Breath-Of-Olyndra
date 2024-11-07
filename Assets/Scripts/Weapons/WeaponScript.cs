using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour{
    
    [Header("Trigger Profiles")]
    [SerializeField] private TriggerProfile _atkTriggerProfile;
    [SerializeField] private TriggerProfile _aimTriggerProfile;

    [Header("Weapon Properties")]
    [SerializeField] private float _damage;
    [SerializeField] private float _atkRate;
    
    private int _weaponIndex;
    
    public void SetWeaponIndex(int val){
        _weaponIndex = val;
    }

    // private void OnEnable() {
    //     UpdateTriggerProfile();
    // }

    // public void UpdateTriggerProfile(){
    //     TriggerManager._current.SetTrigger(_aimTriggerProfile, true);
    //     TriggerManager._current.SetTrigger(_atkTriggerProfile, false);
    // }
}