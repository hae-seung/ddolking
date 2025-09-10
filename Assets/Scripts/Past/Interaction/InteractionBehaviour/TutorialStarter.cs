using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStarter : InteractionBehaviour
{
    [SerializeField] private Transform goalTransform;
    
    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        UIManager.Instance.StartTransition();
        interactor.transform.position = goalTransform.position;

        TutorialManager.Instance.StartTutorial();
    }
    
}
