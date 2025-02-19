using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public void MovePressed(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            GameEventsManager.Instance.inputEvents.MovePressed(context.ReadValue<Vector2>());
        }
    }

    public void InteractPressed(InputAction.CallbackContext context)// F or 마우스 좌클릭
    {
        if (context.started)
        {
            GameEventsManager.Instance.inputEvents.InteractPressed(context);
        }
    }

    public void InventoryToggle(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameEventsManager.Instance.inputEvents.InventoryToggle();
        }
    }

    public void EscPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameEventsManager.Instance.inputEvents.EscPressed();
        }
    }

    public void NumBtnPressed(InputAction.CallbackContext context)
    {
        if (context.started) // 키를 처음 눌렀을 때만 실행
        {
            string key = context.control.name;
            GameEventsManager.Instance.inputEvents.NumBtnPressed(key);
        }
    }
    
    public void MouseMoved(InputAction.CallbackContext context)
    {
        if (context.performed) // 마우스가 움직였을 때만 실행
        {
            Vector2 mousePosition = context.ReadValue<Vector2>();
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePosition);
            GameEventsManager.Instance.inputEvents.MouseMoved(worldMousePos);
        }
    }

}
