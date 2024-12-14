using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public struct AttackType{
    public bool attack;
    public int attackType;//1-basic, 2-strong, 3-AOE
}

public class PlayerInputHandler : MonoBehaviour {
    public static PlayerInputHandler _current;
    private IA_Player _playerInputActions;
    private Vector2 _lookInputRaw;
    public static Vector2 IN_LookInputRaw{
        get { return PlayerInputHandler._current._lookInputRaw; }   // get method
    }

    private Vector2 _moveInputRaw;
    public static Vector2 IN_MoveInputRaw{
        get { return PlayerInputHandler._current._moveInputRaw; }   // get method
    }

    private float _fire;
    public static float IN_IsFiring{
        get { return PlayerInputHandler._current._fire; }   // get method
    }

    private float _aim;
    public static float IN_Aim{
        get { 
            float l_aim = PlayerInputHandler._current._aim;

            if(l_aim < 0.1f)
                return 0f;
            else if(l_aim > 0.9f)
                return 1f;
            
            return l_aim; 
        }
    }
    
    private bool _sprint;
    public static bool IN_IsSprinting{
        get { return PlayerInputHandler._current._sprint; }   // get method
    }
    
    private bool _jump;
    public static bool IN_JumpInput{
        get { return PlayerInputHandler._current._jump; }   // get method
    }
    
    private bool _lockIn;
    public static bool IN_LockInput{
        get { return PlayerInputHandler._current._lockIn; }   // get method
        set { PlayerInputHandler._current._lockIn = value; }
    }

    private bool _lockCycle;
    public static bool IN_LockCycleInput{
        get { return PlayerInputHandler._current._lockCycle; }   // get method
    }
    
    private AttackType _attack;
    public static AttackType IN_Attack{
        get { return PlayerInputHandler._current._attack; }   // get method
    }
    
    private bool _showPlayerControls;
    public static bool IN_ShowControls{
        get { return PlayerInputHandler._current._showPlayerControls; }   // get method
    }
    

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

        _playerInputActions.Player.BasicAttack.started += OnBasicAttackIn;
        _playerInputActions.Player.BasicAttack.performed += OnAttackOut;
        _playerInputActions.Player.BasicAttack.canceled += OnAttackOut;

        _playerInputActions.Player.StrongAttack.performed += OnStrongAttackIn;
        _playerInputActions.Player.StrongAttack.canceled += OnAttackOut;

        _playerInputActions.Player.AOEAttack.performed += OnAOEAttackIn;
        _playerInputActions.Player.AOEAttack.canceled += OnAttackOut;

        _playerInputActions.Player.EnemyLockOn_Cycle.performed += OnEnemyLockInOrCycle;

        _playerInputActions.Player.EnemyLockOut.performed += OnEnemyLockOut;

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

        _playerInputActions.Player.BasicAttack.started -= OnBasicAttackIn;
        _playerInputActions.Player.BasicAttack.performed -= OnAttackOut;

        _playerInputActions.Player.StrongAttack.performed -= OnStrongAttackIn;
        _playerInputActions.Player.StrongAttack.canceled -= OnAttackOut;

        _playerInputActions.Player.AOEAttack.performed -= OnAOEAttackIn;
        _playerInputActions.Player.AOEAttack.canceled -= OnAttackOut;

        _playerInputActions.Player.EnemyLockOn_Cycle.performed += OnEnemyLockInOrCycle;

        _playerInputActions.Player.EnemyLockOut.performed += OnEnemyLockOut;

        //UI
        _playerInputActions.UI.ShowControls.performed -= ShowControls;
        _playerInputActions.UI.ShowControls.canceled -= HideControls;
    }

    private void Update() {
        _lookInputRaw = _playerInputActions.Player.Look.ReadValue<Vector2>();
        _moveInputRaw = _playerInputActions.Player.Movement.ReadValue<Vector2>();
        _aim = _playerInputActions.Player.Aim.ReadValue<float>();
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

        void OnEnemyLockInOrCycle(InputAction.CallbackContext ctx){
            if(!_lockIn){
                PlayerEventSystem._current.CharacterEnemyLockIn();
                _lockIn = true;
            }
            else
                PlayerEventSystem._current.CharacterEnemyLockCycle();
        }

        void OnEnemyLockOut(InputAction.CallbackContext ctx){
            PlayerEventSystem._current.CharacterEnemyLockOut();
            _lockIn = false;
        }

        #region Attacks
            void OnBasicAttackIn(InputAction.CallbackContext ctx){
                _attack.attack = true;
                _attack.attackType = 1;
            }
            void OnStrongAttackIn(InputAction.CallbackContext ctx){
                _attack.attack = true;
                _attack.attackType = 2;
            }
            void OnAOEAttackIn(InputAction.CallbackContext ctx){
                _attack.attack = true;
                _attack.attackType = 3;
            }
            void OnAttackOut(InputAction.CallbackContext ctx){
                _attack.attack = false;
                _attack.attackType = 0;
            }
        #endregion
    #endregion

    #region UI
        void ShowControls(InputAction.CallbackContext ctx){
            _showPlayerControls = true;
        }
        void HideControls(InputAction.CallbackContext ctx){
            _showPlayerControls = false;
        }
    #endregion
}