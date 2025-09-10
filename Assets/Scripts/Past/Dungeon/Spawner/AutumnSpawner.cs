using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class AutumnSpawner : MonoBehaviour, ISpanwBoss
{
    public static AutumnSpawner Instance;
    
    [SerializeField] private List<EntityData> monsterPrefabs;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private int spawnCount;
    [SerializeField] private int monsterFixLevel;



    private Player player;

    private Action KillOneMonster;
    private List<(LivingEntity, Action)> monsters = new();
    
    
    //보스
    private LivingEntity boss;
    private Coroutine bossAppearRoutine;

    public Action DeleteAllMonsters;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        for (int i = 0; i < monsterPrefabs.Count; i++)
        {
            ObjectPoolManager.Instance.RegisterPrefab(monsterPrefabs[i].EntityId, monsterPrefabs[i].EntityPrefab);
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }



    public void SpawnMonsters(Action KillMonster)
    {
        Shuffle(); //몬스터 스폰 포인트 섞기
        KillOneMonster = KillMonster;
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
            KillOneMonster();
        };
        
        livingEntity.onDead += handler;
        monsters.Add((livingEntity, handler));
    }


    public void SpawnBoss(GameObject boss, Transform pos, Action action)
    {
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

        bossAppearRoutine = StartCoroutine(MoveCameraAction(pos));
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
        DeleteAllMonsters?.Invoke(); //보스 패턴으로 나온 몬스터들도 한꺼번에 제거
        
        if(boss != null)
            Destroy(boss);
    }
    
    private IEnumerator MoveCameraAction(Transform bossPos)
    {
        CinemachineVirtualCamera cam = VirtualCameraManager.Instance.GetCamera();
        var transposer = cam.GetCinemachineComponent<CinemachineTransposer>();
        
        // damping 값 저장
        Vector3 originalDamping = new Vector3(
            transposer.m_XDamping,
            transposer.m_YDamping,
            transposer.m_ZDamping
        );
        
        Transform originalTarget = player.transform;
        
        // damping 값을 (1,1,1)로 바꾸기
        transposer.m_XDamping = 0.5f;
        transposer.m_YDamping = 0.5f;
        transposer.m_ZDamping = 1f;

        // 보스에게 카메라 이동
        cam.Follow = bossPos;
        
        
        yield return new WaitForSeconds(3f);
        
        cam.Follow = originalTarget;
        
        yield return new WaitForSeconds(1f);
        
        // damping 원래대로 복원
        transposer.m_XDamping = originalDamping.x;
        transposer.m_YDamping = originalDamping.y;
        transposer.m_ZDamping = originalDamping.z;
    }
}
