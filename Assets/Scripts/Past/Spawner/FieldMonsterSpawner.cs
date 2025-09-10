using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class MonsterProbability
{
    public EntityData monster;
    public int spawnProbability;
}

public class FieldMonsterSpawner : MonoBehaviour
{
    [SerializeField] private List<MonsterProbability> monsterPrefab;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private int spawnStartTime;
    [SerializeField] private int spawnMaxCnt;
    [SerializeField] private Player player;

    [Header("몬스터 강화에 필요한 일 수")]
    [SerializeField] private int remainDay;

    private int remainTime;
    private int curMonsterCnt;
    private int spawnMonsterLevel = 1;
    private bool activeSpawn = true;

    private void Awake()
    {
        SetRandomTime();
    }

    private void SetRandomTime()
    {
        remainTime = Random.Range(1, 3);
    }

    private void Start()
    {
        GameEventsManager.Instance.dayEvents.onChangeTime += ChangeTime;
        GameEventsManager.Instance.dayEvents.onChangeDay += ChangeDay;
    }

    private void ChangeDay(int currentDay)
    {
        if (!activeSpawn)
            return;

        remainDay -= 1;
        if (remainDay <= 0)
        {
            remainDay = 3;
            spawnMonsterLevel++;
        }

        Debug.Log("남은 일수: " + remainDay);
    }

    private void ChangeTime(int currentHour)
    {
        if (!activeSpawn || !IsInNightSpawnTime(currentHour))
            return;

        remainTime--;

        if (remainTime == 0)
        {
            SpawnMonster();
            SetRandomTime();
        }
    }

    private void SpawnMonster()
    {
        if (curMonsterCnt >= spawnMaxCnt)
            return;

        int spawnCount = Random.Range(1, spawnMaxCnt - curMonsterCnt + 1);

        for (int i = 0; i < spawnCount; i++)
        {
            EntityData selected = GetRandomMonsterByProbability();
            if (selected == null) continue;

            SpawnOneMonster(selected);
            curMonsterCnt++;
        }
    }

    private EntityData GetRandomMonsterByProbability()
    {
        int totalWeight = 0;
        foreach (var entry in monsterPrefab)
        {
            totalWeight += entry.spawnProbability;
        }

        if (totalWeight == 0)
            return null;

        int rand = Random.Range(0, totalWeight);
        int cumulative = 0;

        foreach (var entry in monsterPrefab)
        {
            cumulative += entry.spawnProbability;
            if (rand < cumulative)
                return entry.monster;
        }

        return null;
    }

    private void SpawnOneMonster(EntityData data)
    {
        if (!ObjectPoolManager.Instance.IsPoolRegistered(data.EntityId))
        {
            ObjectPoolManager.Instance.RegisterPrefab(data.EntityId, data.EntityPrefab);
        }

        Transform pos = spawnPoints[Random.Range(0, spawnPoints.Count)];
        
        LivingEntity livingEntity = ObjectPoolManager.Instance.SpawnObject(
            data.EntityId,
            pos.position,
            Quaternion.identity).GetComponent<LivingEntity>();

        livingEntity.SetTarget(player, pos);
        livingEntity.SetLevel(spawnMonsterLevel);

        void OnDead()
        {
            livingEntity.onDead -= OnDead;
            curMonsterCnt--;
        }

        livingEntity.onDead += OnDead;
    }

    public void OperateSpawner()
    {
        activeSpawn = true;
    }

    private bool IsInNightSpawnTime(int currentHour)
    {
        int endHour = 4;

        if (spawnStartTime <= endHour)
        {
            return currentHour >= spawnStartTime && currentHour <= endHour;
        }
        else
        {
            return currentHour >= spawnStartTime || currentHour <= endHour;
        }
    }
}
