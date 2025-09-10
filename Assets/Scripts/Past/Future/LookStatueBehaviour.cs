using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookStatueBehaviour : InteractionBehaviour
{
    [SerializeField] protected StatueData statueData;
    
    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        UIManager.Instance.OpenStatueExplain(statueData);
        GameEventsManager.Instance.playerEvents.DisablePlayerMovement();
    }
}
