using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _controlScheme;
    private void Start() {
        _controlScheme.SetActive(false);
    }
    private void Update() {
        // _controlScheme.SetActive(PlayerInputHandler.IN_ShowControls);
    }
}