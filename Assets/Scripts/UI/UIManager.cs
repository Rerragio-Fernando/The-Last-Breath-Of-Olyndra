using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _controlScheme;

    private void Update() {
        _controlScheme.SetActive(PlayerInputHandler._current.GetShowControls());
        Debug.Log(PlayerInputHandler._current.GetShowControls());
    }
}