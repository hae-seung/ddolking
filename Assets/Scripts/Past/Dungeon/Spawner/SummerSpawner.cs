using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class SummerSpawner : MonoBehaviour, ISpanwBoss
{
    [SerializeField] private List<EntityData> monsterPrefabs;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private int spawnCount;
    [SerializeField] private int monsterFixLevel;

    private Action<int> KillFiveMonster;
    private int killMonsterCnt;
    private Player player;

    private List<(LivingEntity, Action)> monsters = new();
    
    
    //보스
    private LivingEntity boss;
    [SerializeField] private float bossCameraLens; //7.5f
    
    
    private void Awake()
    {
        for (int i = 0; i < monsterPrefabs.Count; i++)
        {
            ObjectPoolManager.Instance.RegisterPrefab(monsterPrefabs[i].EntityId, monsterPrefabs[i].EntityPrefab);
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void SpawnMonsters(Action<int> KillMonster)
    {
        Shuffle(); //몬스터 스폰 포인트 섞기
        KillFiveMonster = KillMonster;
        killMonsterCnt = 0;
        for (int i = 0; i < spawnCount; i++)
        {
            Transform pos = spawnPoints[i];
            SpawnOneMonster(pos);
        }
    }

    private void SpawnOneMonster(Transform pos)
    {
        LivingEntity livingEntity = ObjectPoolManager.Instance.SpawnObject(
            monsterPrefabs[Random.Range(0, monsterPrefabs.Count)].EntityId,
            pos.position,
            Quaternion.identity).GetComponent<LivingEntity>();
            
        livingEntity.SetTarget(player, pos);
        livingEntity.SetLevel(monsterFixLevel);


        Action handler = null;
        handler = () =>
        {
            livingEntity.onDead -= handler;
            killMonsterCnt++;
            KillFiveMonster(killMonsterCnt);
        };
        
        livingEntity.onDead += handler;
        monsters.Add((livingEntity, handler));
    }


    public void SpawnBoss(GameObject boss, Transform pos, Action action)
    {
        CinemachineVirtualCamera cam = VirtualCameraManager.Instance.GetCamera();
        cam.m_Lens.OrthographicSize = bossCameraLens;
        
        this.boss = Instantiate(boss, pos).GetComponent<LivingEntity>();
        this.boss.SetTarget(player, pos);
        this.boss.onDead += KillBoss;
        
        UIManager.Instance.OpenBossHealth(this.boss);

        void KillBoss()
        {
            action.Invoke();
            this.boss.onDead -= KillBoss;
            boss = null;
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
        
        if(boss != null)
            Destroy(boss);
    }
    
    private void Shuffle()
    {
        for (int i = spawnPoints.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (spawnPoints[i], spawnPoints[j]) = (spawnPoints[j], spawnPoints[i]);
        }
    }
}
