using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public struct AttackType{
    public bool attack;
    public int attackType;//1-basic, 2-strong, 3-AOE
}

public class PlayerInputHandler : Singleton<PlayerInputHandler> {
    private IA_Player _playerInputActions;
    private bool canAct = true;

    // Movement Events
    public static event Action<Vector2, InputActionPhase> MoveEvent;
    public static event Action<Vector2, InputActionPhase> LookEvent;
    
    public static event Action<InputActionPhase> JumpEvent;
    public static event Action<InputActionPhase> GuardEvent;

    public static event Action<InputActionPhase> BasicAttackEvent;
    public static event Action<InputActionPhase> ManaChargeEvent;
    public static event Action<InputActionPhase> AOEAttackEvent;

    public override void Awake(){
        base.Awake();
        _playerInputActions = new IA_Player();
    }

    private void OnEnable() {
        _playerInputActions.Enable();
    
        // Player input bindings
        BindAction(_playerInputActions.Player.Movement, ctx => InvokeIfCanAct(() => MoveEvent?.Invoke(ctx.ReadValue<Vector2>(), ctx.phase)));
        BindAction(_playerInputActions.Player.Look, ctx => InvokeIfCanAct(() => LookEvent?.Invoke(ctx.ReadValue<Vector2>(), ctx.phase)));

        BindAction(_playerInputActions.Player.Jump, ctx => InvokeIfCanAct(() => JumpEvent?.Invoke(ctx.phase)));

        BindAction(_playerInputActions.Player.Guard, ctx => InvokeIfCanAct(() => GuardEvent?.Invoke(ctx.phase)));

        BindAction(_playerInputActions.Player.BasicAttack, ctx => InvokeIfCanAct(() => BasicAttackEvent?.Invoke(ctx.phase)));
        BindAction(_playerInputActions.Player.ManaCharge, ctx => InvokeIfCanAct(() => ManaChargeEvent?.Invoke(ctx.phase)));
        BindAction(_playerInputActions.Player.AOEAttack, ctx => InvokeIfCanAct(() => AOEAttackEvent?.Invoke(ctx.phase)));
    }

    private void OnDisable() {
        _playerInputActions.Disable();

        UnbindAction(_playerInputActions.Player.Movement, ctx => InvokeIfCanAct(() => MoveEvent?.Invoke(ctx.ReadValue<Vector2>(), ctx.phase)));
        UnbindAction(_playerInputActions.Player.Look, ctx => InvokeIfCanAct(() => LookEvent?.Invoke(ctx.ReadValue<Vector2>(), ctx.phase)));

        UnbindAction(_playerInputActions.Player.Jump, ctx => InvokeIfCanAct(() => JumpEvent?.Invoke(ctx.phase)));

        UnbindAction(_playerInputActions.Player.Guard, ctx => InvokeIfCanAct(() => GuardEvent?.Invoke(ctx.phase)));

        UnbindAction(_playerInputActions.Player.BasicAttack, ctx => InvokeIfCanAct(() => BasicAttackEvent?.Invoke(ctx.phase)));
        UnbindAction(_playerInputActions.Player.ManaCharge, ctx => InvokeIfCanAct(() => ManaChargeEvent?.Invoke(ctx.phase)));
        UnbindAction(_playerInputActions.Player.AOEAttack, ctx => InvokeIfCanAct(() => AOEAttackEvent?.Invoke(ctx.phase)));
    }

    private void InvokeIfCanAct(Action action)
    {
        if (canAct) action?.Invoke();
    }

    private void BindAction(InputAction inputAction, Action<InputAction.CallbackContext> callback)
    {
        inputAction.started += callback;
        inputAction.performed += callback;
        inputAction.canceled += callback;
    }

    private void UnbindAction(InputAction inputAction, Action<InputAction.CallbackContext> callback)
    {
        inputAction.started -= callback;
        inputAction.performed -= callback;
        inputAction.canceled -= callback;
    }
}