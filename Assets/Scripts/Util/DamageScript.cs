using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageScript : MonoBehaviour
{
    [SerializeField] Slider _healthBar;
    [SerializeField] private int _maxHealth;
    private int _health;

    private void Start() {
        _health = _maxHealth;
        _healthBar.maxValue = _maxHealth;
    }

    public void TakeDamage(int val){
        _health -= val;
    }

    private void Update() {
        _healthBar.value = _health;
    }

}