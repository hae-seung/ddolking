using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpringDungeon : Dungeon
{
    //1번방 : 3개의 웨이브
    //2번방 : 2개의 준보스 방

    [Header("스포너")] 
    [SerializeField] private SpringSpawner spawner;

    [TextArea] 
    [SerializeField] private string enterDungeonText;


    [SerializeField] private StageDoorBehaviour stageDoor;
    
    
    private int currentWave;
    private bool isAlarmFinish;


    private int bossCnt;
    private Action ClearBossStage;

    private Action AllClear;
    
    public void Enter(bool hasFirstClear, Action AllClearDungeon)
    {
        //웨이브랑 방 관리
        bossCnt = 2;
        currentWave = 1;
        isAlarmFinish = false;

        AllClear = AllClearDungeon;
        
        if(!hasFirstClear)
        {
            UIManager.Instance.BossAlarm(enterDungeonText);
            StartCoroutine(WaitEnterDungeonAlarm());
            return;
        }
        
        WaveAlarm();
        StartCoroutine(SpawnWave());
    }

    private IEnumerator WaitEnterDungeonAlarm()
    {
        yield return new WaitForSeconds(3.5f);
        WaveAlarm();
        StartCoroutine(SpawnWave());
    }

    public void Exit()
    {
        //전부 없던일로 만들기
        UIManager.Instance.OffAlarms();
        spawner.Off();
    }
    
    private IEnumerator SpawnWave()
    {
        yield return new WaitUntil(() => isAlarmFinish);
        spawner.StartWave(currentWave, ClearWave);
        isAlarmFinish = false;
    }

   
    private void WaveAlarm()
    {
        //몬스터가 들이닥칩니다. 주의하세요. 문구 x3
        UIManager.Instance.WaveAlarm(FinishWaveAlarm);
    }

    private void FinishWaveAlarm()
    {
        isAlarmFinish = true;
    }

    private void ClearWave()
    {
        currentWave++;
        if (currentWave > 3)
        {
            UIManager.Instance.FinishWaveAlarm();
            stageDoor.ClearCurrentStage();
            return;
        }
        
        //다음 웨이브 시작
        WaveAlarm();
        StartCoroutine(SpawnWave());
    }

    public override void SpawnBoss(GameObject boss, Transform spawnPoint, Action ClearBoss)
    {
        ClearBossStage = ClearBoss;
        spawner.SpawnBoss(boss, spawnPoint, KillBoss);
    }

    private void KillBoss()
    {
        bossCnt--;
        
        //나가는 문이 열림.
        ClearBossStage?.Invoke(); 
        
        //만약 보스 2마리를 다 잡았으면 던전 종료
        if (bossCnt <= 0)
        {
            AllClear?.Invoke();
        }
    }
}
