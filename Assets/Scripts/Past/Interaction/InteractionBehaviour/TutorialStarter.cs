using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStarter : InteractionBehaviour
{
    [SerializeField] private Transform goalTransform;
    
    protected override void Interact(Interactor interactor)
    {
        TransitionUI.Instance.EnableTransitionUI();
        interactor.transform.position = goalTransform.position;

        TutorialManager.Instance.StartTutorial();
    }
    
}
