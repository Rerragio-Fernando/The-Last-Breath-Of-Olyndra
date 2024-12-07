using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour {
    public static PlayerInputHandler _current;
    private IA_Player _playerInputActions;
    private Vector2 _lookInputRaw;
    private Vector2 _moveInputRaw;
    private float _fire;
    private float _aim;
    private bool _sprint;
    private bool _jump;
    private bool _attack;
    private bool _showPlayerControls;

    private void Awake() {
        _playerInputActions = new IA_Player();

        if(_current == null)
            _current = this;
        else
            Destroy(this);
    }

    public IA_Player GetInputActionAsset(){
        return _playerInputActions;
    }

    private void OnEnable() {
        _playerInputActions.Player.Enable();
        _playerInputActions.UI.Enable();

        _playerInputActions.Player.Sprint.performed += OnSprintIn;

        _playerInputActions.Player.Jump.performed += OnJumpIn;
        _playerInputActions.Player.Jump.canceled += OnJumpOut;

        _playerInputActions.Player.Attack.started += OnAttackIn;
        _playerInputActions.Player.Attack.canceled += OnAttackOut;

        //UI
        _playerInputActions.UI.ShowControls.performed += ShowControls;
        _playerInputActions.UI.ShowControls.canceled += HideControls;
    }

    private void OnDisable() {
        _playerInputActions.Player.Disable();
        _playerInputActions.UI.Disable();

        _playerInputActions.Player.Sprint.performed -= OnSprintIn;

        _playerInputActions.Player.Jump.performed -= OnJumpIn;
        _playerInputActions.Player.Jump.canceled -= OnJumpOut;

        _playerInputActions.Player.Attack.started -= OnAttackIn;
        _playerInputActions.Player.Attack.canceled -= OnAttackOut;

        //UI
        _playerInputActions.UI.ShowControls.performed -= ShowControls;
        _playerInputActions.UI.ShowControls.canceled -= HideControls;
    }

    private void Update() {
        _lookInputRaw = _playerInputActions.Player.Look.ReadValue<Vector2>();
        _moveInputRaw = _playerInputActions.Player.Movement.ReadValue<Vector2>();
        _aim = _playerInputActions.Player.Aim.ReadValue<float>();
        // Debug.Log(_lookInputRaw);
    }

    //Events

    #region PlayerControls
    //Sprinting
    void OnSprintIn(InputAction.CallbackContext ctx){
        _sprint = !_sprint;
    }

    //Jump
    void OnJumpIn(InputAction.CallbackContext ctx){
        _jump = true;
    }
    void OnJumpOut(InputAction.CallbackContext ctx){
        _jump = false;
    }

    //Switch Weapon
    void OnAttackIn(InputAction.CallbackContext ctx){
        _attack = true;
    }
    void OnAttackOut(InputAction.CallbackContext ctx){
        _attack = false;
    }
    #endregion

    #region UI
    void ShowControls(InputAction.CallbackContext ctx){
        _showPlayerControls = true;
    }
    void HideControls(InputAction.CallbackContext ctx){
        _showPlayerControls = false;
    }
    #endregion

    public Vector2 GetLookInputRaw(){
        return _lookInputRaw;
    }
    public Vector2 GetMoveInputRaw(){
        if(_moveInputRaw.magnitude <= 0f)
            _sprint = false;
        return _moveInputRaw;
    }
    public bool GetSprint(){
        return _sprint;
    }    
    public bool GetJump(){
        return _jump;
    }
    public bool GetAttack(){
        return _attack;
    }
    public bool GetShowControls(){
        return _showPlayerControls;
    }
    public float GetAimValue(){
        if(_aim < 0.1f)
            return 0f;
        else if(_aim > 0.9f)
            return 1f;
        
        return _aim;
    }
}