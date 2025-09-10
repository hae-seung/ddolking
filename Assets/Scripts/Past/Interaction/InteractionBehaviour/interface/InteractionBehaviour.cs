using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionBehaviour : MonoBehaviour, IInteractionBehavior
{
    public void Operate(Interactor interactor, Item currentGripItem = null)
    {
        Interact(interactor, currentGripItem);
    }

    protected abstract void Interact(Interactor interactor, Item currentGripItem = null);
}
