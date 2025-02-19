using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Interactable : MonoBehaviour
{
   public abstract void Interact(Interactor interactor, InputAction.CallbackContext context);

   public abstract void SetInteractState(bool state);
}
