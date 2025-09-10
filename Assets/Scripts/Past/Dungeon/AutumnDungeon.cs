using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class KillText
{
    public int killCount;
    [TextArea] public string bossText;
    public bool printed = false;
}


public class AutumnDungeon : Dungeon
{
    [SerializeField] private AutumnSpawner spawner;
    [SerializeField] private List<KillText> texts;
    [SerializeField] private int bossSpawnNeedCount;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform bossSpawnPoint;

    [SerializeField] private string bossAppearText;
    
    private Action ClearDungeon;
    private int curKillCnt;
    private bool isBossSpawn;
    
    public void Enter(bool hasFirstClear, Action AllClearDungeon)
    {
        Init();
        ClearDungeon = AllClearDungeon;
        
        if(!hasFirstClear)
        {
            StartCoroutine(WaitEnterDungeonAlarm());
            return;
        }

        SpawnMonsters();
    }

    private void Init()
    {
        curKillCnt = 0;
        isBossSpawn = false;
        for (int i = 0; i < texts.Count; i++)
            texts[i].printed = false;
    }

    private IEnumerator WaitEnterDungeonAlarm()
    {
        SpawnMonsters();
        yield return new WaitForSeconds(3.5f);
    }
    
    private void SpawnMonsters()
    {
        //스포너에 소환
        spawner.SpawnMonsters(KillOneMonster);
    }


    private void KillOneMonster()
    {
        curKillCnt++;

        if (curKillCnt >= bossSpawnNeedCount && !isBossSpawn)
        {
            isBossSpawn = true;
            UIManager.Instance.BossAlarm(bossAppearText);
            spawner.SpawnBoss(bossPrefab, bossSpawnPoint, KillBoss);
            return;
        }

        for (int i = 0; i < texts.Count; i++)
        {
            if (curKillCnt >= texts[i].killCount && !texts[i].printed)
            {
                texts[i].printed = true;
                UIManager.Instance.BossAlarm(texts[i].bossText);
                return;
            }
        }
    }
    
    
    public override void SpawnBoss(GameObject boss, Transform spawnPoint, Action ClearBoss)
    {
        //stageDoor에서 호출해야 하는데 가을던전은 door 없느 기믹이라 불필요
    }

    private void KillBoss()
    {
        //모든 몬스터 삭제
        spawner.Off();
    }


    public void Exit()
    {
        //전부 없던일로 만들기
        UIManager.Instance.OffAlarms();
        spawner.Off();
    }
    
}
