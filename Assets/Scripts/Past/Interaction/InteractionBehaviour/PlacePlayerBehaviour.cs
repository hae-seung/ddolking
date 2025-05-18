using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePlayerBehaviour : InteractionBehaviour
{
    [SerializeField] private Transform goalTransform;
    
    protected override void Interact(Interactor interactor)
    {
        UIManager.Instance.StartTransition();
        interactor.transform.position = goalTransform.position;
    }
}
