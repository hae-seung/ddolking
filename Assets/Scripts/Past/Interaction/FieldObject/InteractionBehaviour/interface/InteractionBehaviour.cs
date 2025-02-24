using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionBehaviour : MonoBehaviour, IInteractionBehavior
{
    public void Operate(Interactor interactor)
    {
        Interact(interactor);
    }

    protected abstract void Interact(Interactor interactor);
}
