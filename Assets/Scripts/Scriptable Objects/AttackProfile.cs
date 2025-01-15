using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttacks", menuName = "Attacks")]
public class AttackProfile : ScriptableObject
{
    public GameObject _manaAttackFX;
    public GameObject _normalAttackFX;
    public int _normalAttackDamage;
    public int _manaAttackDamage;
    public float _normalAttackForce;
    public float _manaAttackForce;
    public int _manaCost;
}