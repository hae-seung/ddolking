using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterMineBehaviour : InteractionBehaviour
{
    [SerializeField] private string mineName;
    [TextArea]
    [SerializeField] private string mainSpawnList;
    [SerializeField] private int resetTime;
    [SerializeField] private Transform minePos;
    [SerializeField] private Transform exitPos;


    private int remainTime;

    private Interactor player;
    
    
    private void Awake()
    {
        GameEventsManager.Instance.dayEvents.onChangeTime += ChangeTime;
    }

    protected override void Interact(Interactor interactor)
    {
        player = interactor;
        UIManager.Instance.OpenMineUI(mineName, mainSpawnList, remainTime,EnterMine ,ExitMine);
    }


    private void ChangeTime(int currentTime)
    {
        if (remainTime > 0)
        {
            remainTime -= 1;
        }
    }

    private void EnterMine()
    {
        player.transform.position = minePos.position;
    }
    
    private void ExitMine()
    {
        remainTime = resetTime;
        player.transform.position = exitPos.position;
    }
}