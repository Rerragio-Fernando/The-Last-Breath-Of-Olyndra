using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Transform _cam;

    [Header("Player Properties")]
    [SerializeField] private float _gravity;
    [SerializeField] private float _playerMass;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _dodgeSpeed;
    [SerializeField] private float _dodgeTime;
    [SerializeField] private float _dodgeRate;
    [SerializeField] private float _forwardStepSpeed;
    [SerializeField] private float _playerFriction;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _distanceToGround;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundTrans;
    [SerializeField] private float _turnSmoothTime = 0.5f;

    [Header("Player Weapons")]
    [SerializeField] private GameObject[] _weaponList;

    private float _turnSmoothVelocity = 0f;
    private float _targAngle;
    private float _moveSpeed = 0f;
    private float _nextDodge = 0f;
    private Vector3 _movDir;
    private Vector3 _dodgeDir;
    private bool _isGrounded;
    private int _activeWeaponIndex;
    private bool _jump = false;
    private bool _isGuarding = false;
    private bool _isAttacking = false;
    private Vector3 _velocity;
    private PlayerAnimationScript _playerAnim;
    private CharacterController _cont;
    private GameObject _bossEnemy;
    private PlayerCombatSystem _combatSys;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        _cont = GetComponent<CharacterController>();
        _playerAnim = GetComponent<PlayerAnimationScript>();
        _combatSys = GetComponent<PlayerCombatSystem>();

        _bossEnemy = GameObject.FindWithTag("Enemy");

        PlayerInputHandler.MoveEvent += MoveInput;
        PlayerInputHandler.LookEvent += LookInput;

        PlayerInputHandler.JumpEvent += JumpInput;

        PlayerEventSystem.OnForwardStepEvent += ForwardStep;
        PlayerEventSystem.OnCharacterBoostInEvent += BoostIn;
        PlayerEventSystem.OnCharacterBoostOutEvent += BoostOut;
        PlayerEventSystem.OnCharacterGuardEvent += Guarding;
    }

    #region Input Functions
        private Vector2 _movementIN, _lookIN;
        private bool _aimIN;
        private bool _jumpIN;

        void MoveInput(Vector2 move, InputActionPhase phase){
            if(phase == InputActionPhase.Performed)
                _movementIN = move;
            else
                _movementIN = Vector2.zero;
        }
        void LookInput(Vector2 look, InputActionPhase phase){
            if(phase == InputActionPhase.Performed)
                _lookIN = look;
            else
                _lookIN = Vector2.zero;
        }
        void JumpInput(InputActionPhase phase){
            if(phase == InputActionPhase.Performed)
                _jumpIN = true;
            else
                _jumpIN = false;
        }
    #endregion

    void Guarding(bool val){
        _isGuarding = val;
    }

    private void Update() {
        _isAttacking = _combatSys.IsAttacking;
        Grounded();
        PlayerMovement();
        PlayerLookRotation();

        _cont.Move(_velocity * Time.deltaTime);
    }

    void Grounded(){
        _isGrounded = Physics.CheckSphere(_groundTrans.position, _distanceToGround, _groundLayer);
        _playerAnim.SetGrounded(_isGrounded);
    }
    
    void PlayerMovement(){
        if(!_isGuarding){
            if(!_isAttacking){
                // Calculate movement direction and target angle
                _movDir = new Vector3(_movementIN.x, 0f, _movementIN.y).normalized;
                _targAngle = Mathf.Atan2(_movDir.x, _movDir.z) * Mathf.Rad2Deg + _cam.eulerAngles.y;

                // Handle vertical velocity
                if(!_isGrounded){
                    _velocity.y += -_gravity * _playerMass * Time.deltaTime;
                    _jump = false;
                }
                else if(_jumpIN){
                    PlayerEventSystem.CharacterJump();
                }
                else{
                    _velocity.y = -1f;
                }

                // Handle movement and animation states
                if(_movDir.magnitude > 0f && _isGrounded){
                    _moveSpeed = _runSpeed;
                    PlayerEventSystem.CharacterRun();

                    Vector3 movDir = Quaternion.Euler(0f, _targAngle, 0f) * Vector3.forward;
                    Vector3 adjustedVelocity = movDir.normalized * _moveSpeed;
                    _velocity = new Vector3(adjustedVelocity.x, _velocity.y, adjustedVelocity.z);
                }
                else{
                    ApplyFriction();
                }
            }
            else{
                ApplyFriction();
            }
        }
        else{
            HandleGuarding();
        }

        if(_jump){
            _velocity.y = _jumpForce;
        }
    }

    void ApplyFriction(){
        PlayerEventSystem.CharacterIdle();
        float friction = _isGrounded ? _playerFriction : 0.05f;
        _velocity = Vector3.Lerp(_velocity, new Vector3(0f, _velocity.y, 0f), friction * Time.deltaTime);
    }

    void HandleGuarding(){
        PlayerEventSystem.CharacterIdle();
        // Guarding or dodging logic
        _dodgeDir = new Vector3(_movementIN.x, 0f, _movementIN.y).normalized;

        if(Time.time > _nextDodge && _dodgeDir.magnitude > 0f){
            PlayerEventSystem.CharacterBoostIn();
            _nextDodge = Time.time + 1 / _dodgeRate;
        }
        else{
            PlayerEventSystem.CharacterBoostOut();
        }
    }

    void PlayerLookRotation(){
        float l_angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targAngle, ref _turnSmoothVelocity, _turnSmoothTime);
        Vector3 l_dir = _bossEnemy.transform.position - transform.position;
        l_dir.y = 0f;

        if (_isGuarding || _movDir.magnitude == 0f || _isAttacking)
        {
            transform.rotation = Quaternion.LookRotation(l_dir);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, l_angle, 0f);
        }
    }

    public void JumpForce(){//This is being called by animation events
        _jump = true;
    }

    public void ForwardStep(){
        _velocity += transform.forward * _forwardStepSpeed;
    }

    void BoostIn(){
        float l_tempYVelocity = _velocity.y;
        _velocity = _dodgeDir * _dodgeSpeed;
        _velocity.y = l_tempYVelocity;
    }

    void BoostOut(){
        _velocity = new Vector3(0f, _velocity.y, 0f);
        // Vector3 val = Vector3.Lerp(_velocity, new Vector3(0f, _velocity.y, 0f), _dodgeTime * Time.deltaTime);
    }
}