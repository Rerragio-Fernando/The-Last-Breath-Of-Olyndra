using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour{
    [Header("Weapon Properties")]
    [SerializeField] private int _damage;

    private Collider _weaponCollider;
    private bool _hit;

    private void Start() {
        _weaponCollider = GetComponent<Collider>();
        _weaponCollider.enabled = false;

        _hit = false;
    }

    private void OnDisable() {
        _hit = false;
    }

    private void OnCollisionEnter(Collision other) {
        DamageScript l_ds = other.gameObject.GetComponent<DamageScript>();
        if(l_ds != null && !_hit){
            _hit = true;
            l_ds.TakeDamage(_damage);
        }
    }
}