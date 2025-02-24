using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputEventContext
{
    DEFAULT,
    DIALOGUE
}

public class InputEvents
{
    public InputEventContext inputEventContext { get; private set; } = InputEventContext.DEFAULT;

    
    public event Action<Vector2> onMovePressed;
    public void MovePressed(Vector2 moveDir)//이벤트 실행 매소드
    {
        onMovePressed?.Invoke(moveDir);
    }

    
    public event Action<InputAction.CallbackContext> onInteractPressed;//F : interaction, Dialogue기능은 나중에 추가
    public void InteractPressed(InputAction.CallbackContext context)
    {
        onInteractPressed?.Invoke(context);
    }
    
    public event Action<InputAction.CallbackContext> onReleasePressed;//F : interaction, Dialogue기능은 나중에 추가
    public void RelasePressed(InputAction.CallbackContext context)
    {
        onReleasePressed?.Invoke(context);
    }
    
    
    public event Action onEscPressed; //esc : 취소 버튼
    public void EscPressed()
    {
        onEscPressed?.Invoke();
    }
    
    
    
    public event Action<Vector3> onMouseMoved;
    public void MouseMoved(Vector3 mousePos)
    {
        onMouseMoved?.Invoke(mousePos);
    }

    public event Action onEnableInput;
    public void EnableInput()
    {
        onEnableInput?.Invoke();
    }
    
    public event Action onDisableInput;
    public void DisableInput()
    {
        onDisableInput?.Invoke();
    }

}
