using System;
using Script.Design_Pattern.Tree_Behavious.Dependency_Injection;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, InputController.IPlayerActions
{
    public Vector2 InputMovement { get; set; }
    public Vector2 Look { get; set; }
    public bool IsSprint { get; set; }
    public bool IsAttack { get; set; }
    public bool IsHeavyAttack { get; set; }

    public int SkillNumber { get; set; }
    public event Action JumpAction;
    public event Action DodgeAction;
    public event Action TargetAction;
    public event Action<int> SkillAction;
    
    private InputController inputActions;
    private bool cursorInputForLook = true;
    private bool cursorLocked = true;


    private void Start()
    {
        inputActions = new InputController();
        inputActions.Player.SetCallbacks(this);
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        InputMovement = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        IsSprint = context.performed;
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        TargetAction?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.canceled) { IsAttack = false; }
        else if (context.performed) { IsAttack = true; }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.canceled) { return; }
        JumpAction?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.canceled) { return; }
        DodgeAction?.Invoke();
    }
    
    public void OnSkill(InputAction.CallbackContext context)
    {
        if (context is {canceled: true, performed: true}) { return; }
        if (context.started)
        {
            SkillAction?.Invoke(Convert.ToInt32(context.control.name));
        }
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (context.canceled) { IsHeavyAttack = false; }
        else if (context.performed) { IsHeavyAttack = true; }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {

    }

    public void OnInteract(InputAction.CallbackContext context)
    {

    }



    public void OnLook(InputAction.CallbackContext context)
    {
        if (cursorInputForLook)
        {
            Look = context.ReadValue<Vector2>();
        }
    }



    public void OnNext(InputAction.CallbackContext context)
    {

    }

    public void OnPrevious(InputAction.CallbackContext context)
    {

    }

    public void ApplicationCursor()
    {
        SetCursor(cursorLocked);
    }

    private void SetCursor(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
