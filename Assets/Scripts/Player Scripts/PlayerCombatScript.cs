using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatScript : MonoBehaviour
{   
    [Header("Basic Attack Properties")]
    [SerializeField] private float _basicAttackDamage;

    [Header("AOE Attack Properties")]
    [SerializeField] private float _aoeAttackDamage;

    [Header("Strong Attack Properties")]
    [SerializeField] private float _strongAttackDamage;
    private void Start() {
        // GameEventSystem._current.OnCharacterAttackEvent +=
    }
}