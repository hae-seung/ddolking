using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpringSpawner : MonoBehaviour
{
    private Action clearAction;

    
    //웨이브별 몬스터(3Wave) 
    //준보스 몬스터 2마리

    [SerializeField] private List<EntityData> monsterPrefabs;
    [SerializeField] private List<Transform> spawnPoints;

    //보스 각 방에 한마리씩 소환하고 죽으면 각 방 나갈 수 있도록. onDead설정
    //2마리 다 죽으면 던전 나가라는 알림
    
    
    [SerializeField] private int wave1Cnt;
    [SerializeField] private int wave2Cnt;
    [SerializeField] private int wave3Cnt;

    [SerializeField] private Player player;
    
    [Header("던전의 몬스터는 레벨 고정")]
    [SerializeField] private int monsterFixLevel;
    
    private int currentWave;
    private int curSpawnMonsterCnt;

    private List<(LivingEntity, Action)> monsters = new();

    private void Awake()
    {
        for (int i = 0; i < monsterPrefabs.Count; i++)
        {
            ObjectPoolManager.Instance.RegisterPrefab(monsterPrefabs[i].EntityId, monsterPrefabs[i].EntityPrefab);
        }
    }

    public void StartWave(int curWave, Action ClearWave)
    {
        clearAction = ClearWave;
        currentWave = curWave;

        Shuffle(); //스폰 포인트 한번 섞기
        monsters.Clear();
        
        SpawnMonster();
    }

    private void SpawnMonster()
    {
        int waveCnt = 0;
        switch (currentWave)
        {
            case 1:
                waveCnt = wave1Cnt;
                break;
            case 2:
                waveCnt = wave2Cnt;
                break;
            case 3:
                waveCnt = wave3Cnt;
                break;
        }

        curSpawnMonsterCnt = waveCnt;
        
        for (int i = 0; i < waveCnt; i++)
        {
            Transform pos = spawnPoints[i];
            SpawnOneMonster(pos);
        }
    }

    private void SpawnOneMonster(Transform pos)
    {
        LivingEntity livingEntity = ObjectPoolManager.Instance.SpawnObject(
            monsterPrefabs[currentWave - 1].EntityId,
            pos.position,
            Quaternion.identity).GetComponent<LivingEntity>();
            
        livingEntity.SetTarget(player, pos);
        livingEntity.SetLevel(monsterFixLevel);


        Action handler = null;
        handler = () =>
        {
            livingEntity.onDead -= handler;
            curSpawnMonsterCnt--;

            if (curSpawnMonsterCnt <= 0)
            {
                clearAction?.Invoke();
            }
        };
        
        livingEntity.onDead += handler;
        monsters.Add((livingEntity, handler));
    }

    private void Shuffle()
    {
        for (int i = spawnPoints.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (spawnPoints[i], spawnPoints[j]) = (spawnPoints[j], spawnPoints[i]);
        }
    }


    public void Off()
    {
        foreach (var (entity, handler) in monsters)
        {
            if (entity != null)
                entity.onDead -= handler;

            if (entity.gameObject.activeSelf)
            {
                entity.ReleasePool();
            }
        }
        
        monsters.Clear();
    }
}
