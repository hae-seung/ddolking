using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterMineBehaviour : InteractionBehaviour
{
    [TextArea]
    [SerializeField] private string mineName;
    [TextArea]
    [SerializeField] private string mainSpawnList;
    [SerializeField] private int resetTime;
    [SerializeField] private Transform minePos;
    [SerializeField] private Transform exitPos;
    [SerializeField] private Color mineColor;

    [Header("광산스포너등록")] 
    [SerializeField] private MineSpawner spawner;


    private int remainTime;

    private Interactor player;
    
    
    private void Awake()
    {
        GameEventsManager.Instance.dayEvents.onChangeTime += ChangeTime;
    }

    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        player = interactor;
        UIManager.Instance.OpenMineUI(mineName, mainSpawnList, remainTime, EnterMine ,ExitMine);
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
        GameEventsManager.Instance.playerEvents.MineEnter(mineColor);
        player.transform.position = minePos.position;
        spawner.Spawn();
    }
    
    private void ExitMine()
    {
        GameEventsManager.Instance.playerEvents.ExitMine();
        remainTime = resetTime;
        player.transform.position = exitPos.position;
    }
}