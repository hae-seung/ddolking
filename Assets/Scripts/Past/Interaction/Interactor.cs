using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    private IInteractable interactableObject;
    [SerializeField]private Transform interactionPoint;
    [SerializeField]private float interactionPointRadius = 0.5f;
    [SerializeField]private LayerMask interactableMask;
    [SerializeField] private InteractionPromptUI interactionPromptUI;
    
    private readonly Collider2D[] colliders = new Collider2D[3];
    public int numFound;

    private void Update()
    {
        numFound = Physics2D.OverlapCircleNonAlloc(interactionPoint.position, interactionPointRadius, 
            colliders, interactableMask);

        if (numFound > 0)
        {
            interactableObject = colliders[0].GetComponent<IInteractable>();
            if (interactableObject != null)
            {
                if(!interactionPromptUI.isDisplayed) 
                    interactionPromptUI.SetUp(interactableObject.InteractionPrompt);

                if (Keyboard.current.fKey.wasPressedThisFrame)
                    interactableObject.Interact(this);
            }
        }
        else
        {
            if (interactableObject != null)
                interactableObject = null;
            if(interactionPromptUI.isDisplayed)
                interactionPromptUI.Close();
        }
    }
    
}
