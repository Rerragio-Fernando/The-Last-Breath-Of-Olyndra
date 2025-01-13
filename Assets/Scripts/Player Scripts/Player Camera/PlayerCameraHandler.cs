using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraHandler : MonoBehaviour
{
    [SerializeField] private GameObject _mainBattleCam;
    [SerializeField] private GameObject _ultiCam;

    private void Start() {
        ActivateMainBattleCam();

        PlayerEventSystem.OnUltimateTriggeredIn += ActivateUltiCam;
        PlayerEventSystem.OnUltimateTriggeredOut += ActivateMainBattleCam;
    }

    void ActivateUltiCam(){
        _ultiCam.SetActive(true);
        _mainBattleCam.SetActive(false);
    }

    void ActivateMainBattleCam(){
        _mainBattleCam.SetActive(true);
        _ultiCam.SetActive(false);
    }
}