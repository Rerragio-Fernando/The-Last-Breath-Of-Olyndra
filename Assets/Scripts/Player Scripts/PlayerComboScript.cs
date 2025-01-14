using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerComboScript : MonoBehaviour
{
    [SerializeField] private GameObject _comboPanel;
    [SerializeField] private int _minCombo;
    [SerializeField] private float _comboRetainmentTime;// if the player goes this amount of time without executing a successful hit then reset combo to 0

    private int _combo;
    public int Combo{
        get{return _combo;}
    }
    
    private float _comboTimer = 0f;
    private TextMeshProUGUI _comboText;

    private void Start() {
        _comboText = _comboPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        ResetCombo();
        PlayerEventSystem.OnResetComboEvent += ResetCombo;
        PlayerEventSystem.OnSuccessfulHitEvent += SuccessfulHit;
    }

    private void Update() {
        if(Time.time > _comboTimer)
            ResetCombo();
    }

    void SuccessfulHit(){
        _comboTimer = Time.time + _comboRetainmentTime;
        _combo++;

        UpdateComboPanel();

        if(_combo > _minCombo)
            PlayerEventSystem.TriggerFocusCamIn();
    }

    void ResetCombo(){
        _combo = 0;
        UpdateComboPanel();
        PlayerEventSystem.TriggerFocusCamOut();
    }

    void UpdateComboPanel(){
        if(_combo == 0)
            _comboPanel.SetActive(false);
        else{
            if(!_comboPanel.activeSelf)
                _comboPanel.SetActive(true);

            _comboText.text = _combo.ToString() + " Hit";
        }
    }
}