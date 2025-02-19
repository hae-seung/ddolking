using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableObject : Interactable
{
    [SerializeField] private GameObject interactionUI;

    private void Awake()
    {
        SetInteractState(false);
    }

    public override void Interact(Interactor interactor, InputAction.CallbackContext context)
    {
        if (context.control.device is Keyboard)
        {
            // F키 눌림 의미 todo: 상호작용 : NPC 대화, 장소이동, 제작탭 열기 등등
        }
        
    }

    public override void SetInteractState(bool state)
    {
        interactionUI.SetActive(state);
    }
}
