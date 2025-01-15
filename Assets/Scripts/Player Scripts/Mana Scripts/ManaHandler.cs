using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaHandler : MonoBehaviour
{
    [Header("Mana Properties")]
    [SerializeField] private Slider _manaSlider;
    [SerializeField] private int _maxMana;
    public int PlayerMaxMana{
        get{return _maxMana;}
    }

    private int _playerMana;
    public int PlayerMana{
        get{return _playerMana;}
        set{_playerMana = value;}
    }

    private float _nextManaReCharge = 0f;

    private void Start() {
        _playerMana = _maxMana;
        _manaSlider.maxValue = _maxMana;
    }

    private void Update() {
        _manaSlider.value = _playerMana;
    }

    public void UseMana(int val){
        _playerMana -= val;
    }

    public void GainMana(int val){
        _playerMana += val;

        if(_playerMana > _maxMana)
            _playerMana = _maxMana;
    }
}