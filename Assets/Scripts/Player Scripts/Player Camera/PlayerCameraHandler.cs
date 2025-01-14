using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraHandler : MonoBehaviour
{
    [SerializeField] private GameObject _mainBattleCam;
    [SerializeField] private GameObject _ultiCam;
    [SerializeField] private GameObject _focusCam;

    private void Start() {
        ActivateMainBattleCam();

        PlayerEventSystem.OnUltimateTriggeredIn += ActivateUltiCam;
        PlayerEventSystem.OnUltimateTriggeredOut += ActivateMainBattleCam;

        PlayerEventSystem.OnBattleFocusInEvent += ActivateFocusCam;
        PlayerEventSystem.OnBattleFocusOutEvent += DeactivateFocusCam;
    }

    void ActivateUltiCam(){
        _ultiCam.SetActive(true);
        _focusCam.SetActive(false);
        _mainBattleCam.SetActive(false);
    }

    void ActivateMainBattleCam(){
        _mainBattleCam.SetActive(true);
        _focusCam.SetActive(false);
        _ultiCam.SetActive(false);
    }

    void ActivateFocusCam(){
        _focusCam.SetActive(true);
    }

    void DeactivateFocusCam(){
        _focusCam.SetActive(false);
    }
}