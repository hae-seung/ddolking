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

    public void InteractPressed(InputAction.CallbackContext context)
    {
        bool isMouseClick = context.control.name.Equals("leftButton");

        if (isMouseClick)
        {
            if (context.started) // 한 번 클릭
            {
                GameEventsManager.Instance.inputEvents.InteractPressed(context);
            }
            else if (context.performed) // 0.2초 이상 꾹 눌렀을 때
            {
                GameEventsManager.Instance.inputEvents.InteractPressed(context);
            }
            else if (context.canceled) // 마우스를 떼었을 때
            {
                GameEventsManager.Instance.inputEvents.InteractPressed(context);
            }
        }
        else // TAB, F 같은 다른 키는 한 번만 실행
        {
            if (context.started)
            {
                GameEventsManager.Instance.inputEvents.InteractPressed(context);
            }
        }
    }


    public void EscPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameEventsManager.Instance.inputEvents.EscPressed();
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
