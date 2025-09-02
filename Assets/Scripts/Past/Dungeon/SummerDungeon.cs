using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummerDungeon : Dungeon
{
    [SerializeField] private SummerSpawner spawner;
    [TextArea] [SerializeField] private string enterDungeonText;
    [Header("보스입장에 필요한 몬스터 킬 수")]
    [SerializeField] private int howKillMonster;
    [SerializeField] private StageBossDoorBehaviour bossDoor;


    private Action ClearDungeon;
    
    //미로는 몬스터를 최소 5마리는 잡아야 탈출 가능
    public void Enter(bool hasFirstClear, Action AllClearDungeon)
    {
        ClearDungeon = AllClearDungeon;
        
        if(!hasFirstClear)
        {
            UIManager.Instance.BossAlarm(enterDungeonText);
            StartCoroutine(WaitEnterDungeonAlarm());
            return;
        }

        SpawnMazeMonsters();
    }

    private void SpawnMazeMonsters()
    {
        spawner.SpawnMonsters(KillFiveMonsters);
    }


    private IEnumerator WaitEnterDungeonAlarm()
    {
        SpawnMazeMonsters();
        yield return new WaitForSeconds(3.5f);
    }

    private void KillFiveMonsters(int cnt)
    {
        //5마리를 다 죽이게 되었을때 보스방 입장가능
        if (cnt >= howKillMonster)
        {
            bossDoor.ClearCurrentStage();
        }
    }
    
    public override void SpawnBoss(GameObject boss, Transform spawnPoint, Action ClearBoss)
    {
        //나가는 문 필요 x
        spawner.SpawnBoss(boss, spawnPoint, KillBoss);
    }

    private void KillBoss()
    {
        ClearDungeon.Invoke();
    }

    public void Exit()
    {
        //전부 없던일로 만들기
        UIManager.Instance.OffAlarms();
        spawner.Off();
    }
}
