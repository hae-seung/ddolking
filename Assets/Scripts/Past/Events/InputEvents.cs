using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    
    public event Action onInteractPressed;//F : interaction, Dialogue기능은 나중에 추가
    public void InteractPressed()
    {
        onInteractPressed?.Invoke();
    }

    
    public event Action onInventoryToggle;//tab : 인벤토리 열기
    public void InventoryToggle()
    {
        onInventoryToggle?.Invoke();
    }
    
    
    public event Action onEscPressed; //esc : 취소 버튼
    public void EscPressed()
    {
        onEscPressed?.Invoke();
    }

    
    
}
