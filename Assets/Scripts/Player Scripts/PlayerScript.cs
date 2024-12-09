using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Transform _cam;

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

    protected Vector2 _lookIN;

    private bool _jump = false;
    private Vector3 _velocity;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        _cont = GetComponent<CharacterController>();
        _playerAnim = GetComponent<PlayerAnimationScript>();

        // PlayerEventSystem._current.OnAnimationJumpForceEvent += JumpForce;
        PlayerEventSystem._current.OnCharacterBoostInEvent += BoostIn;
        PlayerEventSystem._current.OnCharacterBoostOutEvent += BoostOut;
    }

    private void Update() {
        Grounded();
        PlayerMovement();
        PlayerLookRotation();
    }

    void Grounded(){
        _isGrounded = Physics.CheckSphere(_groundTrans.position, _distanceToGround, _groundLayer);
        _playerAnim.SetGrounded(_isGrounded);
    }
    void PlayerLookRotation(){
        float l_angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targAngle, ref _turnSmoothVelocity, _turnSmoothTime);
        if(PlayerInputHandler.IN_Aim > 0f){
            PlayerEventSystem._current.CharacterAimIn();
            if(_targAngle != l_angle)
                transform.rotation = Quaternion.Euler(0f, _cam.eulerAngles.y, 0f);
            this.transform.Rotate(Vector3.up * (PlayerInputHandler.IN_LookInputRaw.x * _aimSensitivity * Time.deltaTime));
        }
        else{
            if(!PlayerInputHandler.IN_LockInput){
                PlayerEventSystem._current.CharacterAimOut();
                if(_movDir.magnitude > 0f){
                    transform.rotation = Quaternion.Euler(0f, l_angle, 0f);
                }
            }
            else{
                if(_movDir.magnitude <= 0f)
                    transform.LookAt(PlayerLockOnScript.currentFocusedEnemy.transform, Vector3.up);
                else
                    transform.rotation = Quaternion.Euler(0f, l_angle, 0f);
            }
        }
    }
    
    void PlayerMovement(){
        _movDir = new Vector3(PlayerInputHandler.IN_MoveInputRaw.x, 0f, PlayerInputHandler.IN_MoveInputRaw.y).normalized;
        _targAngle = Mathf.Atan2(_movDir.x, _movDir.z) * Mathf.Rad2Deg + _cam.eulerAngles.y;
        
        if(!_isGrounded){
            _velocity.y += -_gravity * _playerMass * Time.deltaTime;
            _jump = false;
        }
        else{
            if(PlayerInputHandler.IN_JumpInput){
                // JumpForce();
                PlayerEventSystem._current.CharacterJump();
            }
            else{
                _velocity.y = 0f;
            }
        }
        
        if(_movDir.magnitude > 0f && _isGrounded){            
            if(PlayerInputHandler.IN_Aim > 0f){ //if Player Aiming
                l_moveSpeed = _aimMoveSpeed;
                _playerAnim.UpdateCharacterDirection(PlayerInputHandler.IN_MoveInputRaw);
            }

            if(PlayerInputHandler.IN_IsSprinting){
                if(PlayerInputHandler.IN_Aim <= 0f)
                    l_moveSpeed = _runSpeed;
                PlayerEventSystem._current.CharacterRun();
            }
            else{
                l_moveSpeed = _walkSpeed;
                PlayerEventSystem._current.CharacterWalk();
            }

            Vector3 movDir = Quaternion.Euler(0f, _targAngle, 0f) * Vector3.forward;  
            Vector3 l_temp = (movDir.normalized * l_moveSpeed);
            _velocity = new Vector3(l_temp.x, _velocity.y, l_temp.z);
        }
        else{
            PlayerEventSystem._current.CharacterIdle();
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

    void JumpForce(){//This is being called by animation events
        _jump = true;
    }

    void BoostIn(){
        float l_tempYVelocity = _velocity.y;
        _velocity = (PlayerLockOnScript.currentFocusedEnemy.transform.position - transform.position) * 50f;
        // _velocity.y = l_tempYVelocity;
    }

    void BoostOut(){
        _velocity.x = _velocity.z = 0f;
    }
}