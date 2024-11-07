using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler _input;
    [SerializeField] private Transform _cam;
    [SerializeField] private Transform _playerGraphics;

    [Header("Player Properties")]
    [SerializeField] private float _gravity;
    [SerializeField] private float _playerMass;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _aimMoveSpeed;
    [SerializeField] private float _playerFriction;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _aimToMainMovementSwitchDelay;
    [SerializeField] private float _aimSensitivity;
    [SerializeField] private float _distanceToGround;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundTrans;
    [SerializeField] private float _turnSmoothTime = 0.5f;

    [Header("Player Weapons")]
    [SerializeField] private GameObject[] _weaponList;

    private float _turnSmoothVelocity = 0f;
    private float _targAngle;
    private float l_moveSpeed = 0f;
    private Vector3 _movDir;
    private bool _isGrounded;
    private int _activeWeaponIndex;
    private PlayerAnimationScript _playerAnim;
    private CharacterController _cont;

    private bool _jump = false;

    //Input Variables
    private bool _sprintIN, _jumpIN, _changeWeaponIN;
    private bool _toggleChangeWeapon = false;
    private float _aimValue;
    private Vector2 _movementIN, _lookIN;
    private Vector3 _velocity;

    private void Awake() {
        _activeWeaponIndex = 0;
        InitializeActiveWeapon();
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;

        _cont = GetComponent<CharacterController>();
        _playerAnim = GetComponent<PlayerAnimationScript>();

        GameEventSystem._current.OnAnimationJumpForceEvent += JumpForce;
    }

    private void Update() {
        Grounded();
        UpdateInputVariables();
        PlayerLook();
        PlayerMovement();
        Combat();
    }

    void Grounded(){
        _isGrounded = Physics.CheckSphere(_groundTrans.position, _distanceToGround, _groundLayer);
        _playerAnim.SetGrounded(_isGrounded);
    }

    void UpdateInputVariables(){
        _sprintIN = _input.GetSprint();
        _jumpIN = _input.GetJump();
        _movementIN = _input.GetMoveInputRaw();
        _lookIN = _input.GetLookInputRaw();
        _changeWeaponIN = _input.GetSwitchWeapon();
        _aimValue = _input.GetAimValue();
    }
    void PlayerLook(){
        float l_angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targAngle, ref _turnSmoothVelocity, _turnSmoothTime);
        if(_aimValue > 0f){
            GameEventSystem._current.CharacterAimIn();
            if(_targAngle != l_angle)
                transform.rotation = Quaternion.Euler(0f, _cam.eulerAngles.y, 0f);
            this.transform.Rotate(Vector3.up * (_lookIN.x * _aimSensitivity * Time.deltaTime));
        }
        else{
            GameEventSystem._current.CharacterAimOut();
            if(_movDir.magnitude > 0f){
                transform.rotation = Quaternion.Euler(0f, l_angle, 0f);
            }
        }
    }
    
    void PlayerMovement(){
        _movDir = new Vector3(_movementIN.x, 0f, _movementIN.y).normalized;
        _targAngle = Mathf.Atan2(_movDir.x, _movDir.z) * Mathf.Rad2Deg + _cam.eulerAngles.y;
        
        if(!_isGrounded){
            _velocity.y += -_gravity * _playerMass * Time.deltaTime;
            _jump = false;
        }
        else{
            if(_jumpIN)
                GameEventSystem._current.CharacterJump();
            else{
                _velocity.y = 0f;
            }
        }
        
        if(_movDir.magnitude > 0f && _isGrounded){            
            if(_aimValue > 0f){ //if Player Aiming
                l_moveSpeed = _aimMoveSpeed;
                _playerAnim.UpdateCharacterDirection(_movementIN);
            }

            if(_sprintIN){
                if(_aimValue <= 0f)
                    l_moveSpeed = _runSpeed;
                GameEventSystem._current.CharacterRun();
            }
            else{
                l_moveSpeed = _walkSpeed;
                GameEventSystem._current.CharacterWalk();
            }

            Vector3 movDir = Quaternion.Euler(0f, _targAngle, 0f) * Vector3.forward;  
            Vector3 l_temp = (movDir.normalized * l_moveSpeed);
            _velocity = new Vector3(l_temp.x, _velocity.y, l_temp.z);
        }
        else{
            GameEventSystem._current.CharacterIdle();
            float l_friction;

            if(_isGrounded)
                l_friction = _playerFriction;
            else
                l_friction = .05f;

            _velocity = Vector3.Lerp(_velocity, new Vector3(0f, _velocity.y, 0f), l_friction * Time.deltaTime);
        }

        if(_jump){
            _velocity.y = _jumpForce;
        }

        _cont.Move(_velocity * Time.deltaTime);
    }

    void Combat(){
        if(_changeWeaponIN){
            if(!_toggleChangeWeapon){
                _toggleChangeWeapon = true;
                SwitchWeapon();   
            }
        }
        else
            _toggleChangeWeapon = false;
    }

    void JumpForce(){//This is being called by animation events
        Debug.Log($"Pamka");
        _jump = true;
    }

    void InitializeActiveWeapon(){
        foreach (var item in _weaponList)
        {
            item.SetActive(false);
        }

        _weaponList[_activeWeaponIndex].SetActive(true);
    }

    void SwitchWeapon(){
        int temp = _activeWeaponIndex;
        _activeWeaponIndex++;

        if(_activeWeaponIndex > _weaponList.Length - 1)
            _activeWeaponIndex = 0;

        _weaponList[temp].SetActive(false);
        _weaponList[_activeWeaponIndex].SetActive(true);
    }
}