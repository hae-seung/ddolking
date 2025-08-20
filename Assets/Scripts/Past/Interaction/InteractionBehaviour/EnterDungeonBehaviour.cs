using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DungeonType
{
    Spring,
    Summer,
    Autumn,
    Winter
}


[System.Serializable]
public class SweepReward
{
    public ItemData item;
    public int amount;
}

public class EnterDungeonBehaviour : InteractionBehaviour
{
    [TextArea]
    [SerializeField] private string dungeonName;
    [TextArea]
    [SerializeField] private string dungeonExplain;
    [SerializeField] private int resetTime;
    [SerializeField] private int sweepLimitTime;
    [SerializeField] private Transform dungeonPos;
    [SerializeField] private Transform exitPos;
    [SerializeField] private DungeonType _dungeonType;

    [Header("소탕리워드")] 
    [SerializeField] private List<SweepReward> _rewards;

    [Header("던전 밝기")] 
    [SerializeField] private Color dungeonLight;
    
    //소탕보상필요, UI필요
    
    private int remainTime;
    private Interactor player;

    [SerializeField]
    private bool hasFirstClear;
    [SerializeField]
    private bool isSweepable;
    
    private void Awake()
    {
        GameEventsManager.Instance.dayEvents.onChangeTime += ChangeTime;
    }
    
    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        player = interactor;
        UIManager.Instance.OpenDungeonUI(dungeonName, dungeonExplain, remainTime, sweepLimitTime,
            isSweepable, hasFirstClear,
            EnterDungeon, ExitDungeon);
        UIManager.Instance.OpenDungeonSweepList(isSweepable, _rewards);
    }
    
    private void ChangeTime(int currentTime)
    {
        if (remainTime > 0)
        {
            remainTime -= 1;
        }
    }

    private void EnterDungeon()
    {
        StartCoroutine(StartTransition());
    }

    private IEnumerator StartTransition()
    {
        yield return new WaitForSeconds(0.5f);
        
        UIManager.Instance.StartTransition();
        player.transform.position = dungeonPos.position;
        GameEventsManager.Instance.playerEvents.MineEnter(dungeonLight);
        DungeonManager.Instance.EnterDungeon(_dungeonType, hasFirstClear, ClearDungeon);

        VirtualCameraManager.Instance.GetCamera(CameraType.dungeon).SetActive(true);
        if (hasFirstClear && !isSweepable)
        {
            //DungeonManger에게 타이머도 띄워달라고 부탁하기
            UIManager.Instance.StartDungeonTimer(sweepLimitTime);
        }
    }

    private void ExitDungeon()
    {
        GameEventsManager.Instance.playerEvents.ExitMine();
        player.transform.position = exitPos.position;
        remainTime = resetTime;
        DungeonManager.Instance.ExitDungeon(_dungeonType);
        VirtualCameraManager.Instance.GetCamera(CameraType.dungeon).SetActive(false);
    }

    private void ClearDungeon()
    {
        UIManager.Instance.StopTimer();
        
        if(UIManager.Instance.GetRemainTime() > 0)
        {
            if (!hasFirstClear)
            {
                hasFirstClear = true;
                return;
            }

            if (!isSweepable)
            {
                isSweepable = true;
                return;
            }
        }
    }
}
