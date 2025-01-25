using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInteraction : Interactable
{
    [SerializeField] private Transform targetTransform;
    public override void Interact(Interactor interactor)
    {
        interactor.transform.position = targetTransform.position;
    }
}
