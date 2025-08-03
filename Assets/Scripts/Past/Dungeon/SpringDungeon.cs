using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringDungeon : MonoBehaviour
{
    //1번방 : 3개의 웨이브
    //2번방 : 2개의 준보스 방

    [Header("스포너")] 
    [SerializeField] private SpringSpawner spawner;

    [TextArea] 
    [SerializeField] private string enterDungeon;


    [SerializeField] private StageDoorBehaviour stageDoor;
    
    private int currentWave;
    private bool isAlarmFinish;
    
    
    public void Enter(bool hasFirstClear)
    {
        //웨이브랑 방 관리
        currentWave = 1;
        isAlarmFinish = false;
        
        if(!hasFirstClear)
        {
            UIManager.Instance.BossAlarm(enterDungeon);
            StartCoroutine(WaitBossAlarm());
            return;
        }
        
        WaveAlarm();
        StartCoroutine(SpawnWave());
    }

    private IEnumerator WaitBossAlarm()
    {
        yield return new WaitForSeconds(4f);
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
}
