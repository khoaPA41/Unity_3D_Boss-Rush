using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, InputController.IPlayerActions
{

    public Vector2 InputMovement { get; private set; }
    public bool IsSprint { get; private set; }
    public bool IsAttack { get; private set; }

    public event Action JumpAction;

    public event Action TargetAction;


    InputController inputActions;


    void Start()
    {
        inputActions = new InputController();
        inputActions.Player.SetCallbacks(this);
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        InputMovement = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsSprint = true;
        }
        else
        {
            IsSprint = false;
        }
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

    public void OnCrouch(InputAction.CallbackContext context)
    {

    }

    public void OnInteract(InputAction.CallbackContext context)
    {

    }



    public void OnLook(InputAction.CallbackContext context)
    {

    }



    public void OnNext(InputAction.CallbackContext context)
    {

    }

    public void OnPrevious(InputAction.CallbackContext context)
    {

    }


}
