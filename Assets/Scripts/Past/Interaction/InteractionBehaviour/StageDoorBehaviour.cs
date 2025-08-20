using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDoorBehaviour : InteractionBehaviour
{
    [SerializeField]
    protected bool canEnter;
    

    [TextArea] [Header("없다면 공백으로")]
    [SerializeField] private string enterMessage;
    [SerializeField] private Transform nextStagePos;
    

    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        if (!canEnter)
            return;

        UIManager.Instance.OffAlarms();
        
        interactor.transform.position = nextStagePos.position;
        UIManager.Instance.StartTransition();
        UIManager.Instance.BossAlarm(enterMessage);
    }

    public void ClearCurrentStage()
    {
        canEnter = true;
    }

    public void BlockDoor()
    {
        canEnter = false;
    }
    
}