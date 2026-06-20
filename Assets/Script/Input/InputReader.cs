using System;
using Script.Design_Pattern.Tree_Behavious.Dependency_Injection;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, InputController.IPlayerActions
{
    public Vector2 InputMovement { get; private set; }
    public Vector2 Look { get; set; }
    public bool IsSprint { get; private set; }
    public bool IsAttack { get; set; }
    public bool IsHeavyAttack { get; set; }

    public event Action JumpAction;
    public event Action DodgeAction;
    public event Action TargetAction;
    public event Action UsePotionAction;
    public event Action ChangeHealthPotionAction;
    public event Action ChangeManaPotionAction;
    public event Action UseSubPotionAction;
    public event Action ChangeNextSubPotionAction;
    public event Action ChangePrevSubPotionAction;
    public event Action<int> SkillAction;


    private InputController inputActions;
    private bool cursorInputForLook = true;
    private bool cursorLocked = true;
    private InputBuffering _inputBuffering;
    private void Awake()
    {
        _inputBuffering = GetComponent<InputBuffering>();
        inputActions = new InputController();
        inputActions.Player.SetCallbacks(this);
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void DisableInput()
    {
        inputActions.Disable();
    }
    
    public void OnEnableInput()
    {
        inputActions.Enable();
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
        else if (context.performed)
        {
            _inputBuffering.Register(ActionType.Attack);
            IsAttack = true; 
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.canceled) { return; }
        _inputBuffering.Register(ActionType.Jump);
        JumpAction?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.canceled) { return; }
        _inputBuffering.Register(ActionType.Dodge);
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

    public void OnUsePotion(InputAction.CallbackContext context)
    {
        if (context is {canceled: true, performed: true}) return;
        if (Keyboard.current != null && Keyboard.current.altKey.isPressed)
        {
            Debug.Log("Sub Potion");
            if(context.started) UseSubPotionAction?.Invoke();
            return;
        }
        
        if (context.started)
        {
            UsePotionAction?.Invoke();
        }
    }
    
    

    public void OnChangeMainPotion(InputAction.CallbackContext context)
    {
        if ((Keyboard.current != null && Keyboard.current.qKey.isPressed) && !context.canceled)
        {
            var scrollY = context.ReadValue<Vector2>();
            if (!context.performed) return;
            switch (scrollY.y)
            {
                case > 0:
                    Debug.Log("Next Main Potion");
                    ChangeHealthPotionAction?.Invoke();
                    return;
                case < 0:
                    Debug.Log("Prev Main Potion");
                    ChangeManaPotionAction?.Invoke();
                    return;
            }
        }

        if ((Keyboard.current != null && Keyboard.current.altKey.isPressed) && !context.canceled)
        {
            var scrollY = context.ReadValue<Vector2>();
            if (!context.performed) return;
            switch (scrollY.y)
            {
                case > 0:
                    Debug.Log("Next Main Potion");
                    ChangeNextSubPotionAction?.Invoke();
                    return;
                case < 0:
                    Debug.Log("Prev Main Potion");
                    ChangePrevSubPotionAction?.Invoke();
                    return;
            }
        }
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
