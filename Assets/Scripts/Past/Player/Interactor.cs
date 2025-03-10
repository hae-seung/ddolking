using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    private Interactable interactableObject;
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask interactableMask;

    private readonly Collider2D[] colliders = new Collider2D[5];
    public int numFound;
    
    private void Update()
    {
        numFound = Physics2D.OverlapCircleNonAlloc(interactionPoint.position, interactionPointRadius,
            colliders, interactableMask);

        if (numFound > 0)
        {
            Interactable newInteractable = colliders[0].GetComponent<Interactable>();

            // 새로운 오브젝트에 접근했을 때만 UI 변경
            if (newInteractable != null && newInteractable != interactableObject)
            {
                if (interactableObject != null) 
                    interactableObject.SetInteractState(false); // 이전 UI 끄기

                interactableObject = newInteractable;
                interactableObject.SetInteractState(true); // 새로운 UI 켜기
            }
        }
        else
        {
            // 범위를 벗어나면 UI를 한 번만 끄기
            if (interactableObject != null)
            {
                interactableObject.SetInteractState(false);
                interactableObject = null;
            }
        }
    }

    public void InteractPressed(InputAction.CallbackContext context, Item currentGripItem)
    {
        if (numFound > 0 && interactableObject != null)
        {
            interactableObject.Interact(this, context, currentGripItem);
        }
    }
}