using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
public class TalkBehaviour : InteractionBehaviour
{
    [SerializeField] private List<TalkSessionData> talkSessionDatas;
    protected override void Interact(Interactor interactor)
    {
        
    }
}
