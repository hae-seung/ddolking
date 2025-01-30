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
        if (context.started)
        {
            GameEventsManager.Instance.inputEvents.InteractPressed();
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
}
