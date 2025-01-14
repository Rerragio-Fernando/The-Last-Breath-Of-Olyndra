using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamageScript : DamageScript
{
    [Header("Orb Drop Parameters")]
    [SerializeField] private float _damageInfluence = 0.5f;
    [SerializeField] private float _comboInfluence = 0.5f;

    EnemyHealth health;

    public int TakeDamage(int dmg, int comboNumber){//returns mana gained
        base.TakeDamage(dmg);
        int l_manaGain = (int)((_damageInfluence * dmg) + (_comboInfluence * comboNumber));
        health = health == null ? EnemyHealth.instance : health;
        health.applyDamage(dmg);

        return l_manaGain;
    }
}