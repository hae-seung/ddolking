using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class SpawnProbability
{
    [SerializeField] private FieldObjectData spawnItem;
    [Header("%")]
    [SerializeField] private int probability;
    
    public int Probability => probability;
    public FieldObjectData FieldObject => spawnItem;
}


public class SpawnObject
{
    private int id;
    private GameObject fieldObject;

    public SpawnObject(int id, GameObject obj)
    {
        this.id = id;
        fieldObject = obj;
    }

    public void ReleasePool()
    {
        if (!fieldObject.activeSelf)
            return;
        
        ObjectPoolManager.Instance.ReleaseObject(id, fieldObject);
    }
}

public class MineSpawner : MonoBehaviour
{
    [Header("스폰 포인트")]
    [SerializeField] private GameObject spawnPointsParent;

    [Header("스폰 아이템과 확률")] 
    [SerializeField] private List<SpawnProbability> spawnItems;

    [SerializeField] private int minSpawnCount;
    [SerializeField] private int maxSpawnCount;
    
    [Header("몬스터 스폰 포인트")] 
    [SerializeField] private GameObject monsterSpawnPointsParent;

    [Header("몬스터 프리팹")] 
    [SerializeField] private List<EntityData> monsterPrefabs;

    [SerializeField] private int monsterSpawnCountMin;
    [SerializeField] private int monsterSpawnCountMax;

    [SerializeField] private int monsterFixLevel;

    private Player player;
    private List<SpawnObject> currentObjects;
    private List<(LivingEntity, Action)> monsters;
    private List<Transform> spawnPoints = new List<Transform>();
    private List<Transform> monsterSpawnPoints = new List<Transform>();
    
    private int totalPb;

    private void Start()
    {
        currentObjects = new List<SpawnObject>();
        monsters = new List<(LivingEntity, Action)>();
        
        for (int i = 0; i < spawnPointsParent.transform.childCount; i++)
        {
            spawnPoints.Add(spawnPointsParent.transform.GetChild(i));
        }

        for (int i = 0; i < monsterSpawnPointsParent.transform.childCount; i++)
        {
            monsterSpawnPoints.Add(monsterSpawnPointsParent.transform.GetChild(i));
        }
        
        for(int i = 0; i < spawnItems.Count; i++)
        {
            totalPb += spawnItems[i].Probability;
            ObjectPoolManager.Instance.RegisterPrefab(spawnItems[i].FieldObject.id, 
                spawnItems[i].FieldObject.ownObject);
        }

        for (int i = 0; i < monsterPrefabs.Count; i++)
        {
            ObjectPoolManager.Instance.RegisterPrefab(monsterPrefabs[i].EntityId, monsterPrefabs[i].EntityPrefab);
        }
    }

    public void Spawn(Interactor player)
    {
        if (this.player == null)
            this.player = player.GetComponent<Player>();
        
        
        int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);
        Shuffle(spawnPoints);
        
        for (int i = 0; i < spawnCount; i++)
        {
            FieldObjectData item = GetRandomItem();
            
            if (item == null) continue;
            
            GameObject obj = ObjectPoolManager.Instance.SpawnObject(
                item.id,
                spawnPoints[i].position,
                Quaternion.identity);

            SpawnObject newSpawnObject = new SpawnObject(item.id, obj);
            currentObjects.Add(newSpawnObject);
        }

        SpawnMonsters();
    }

    public void Exit()
    {
        DestroyObject();
    }

    private void SpawnMonsters()
    {
        int count = Random.Range(monsterSpawnCountMin, monsterSpawnCountMax + 1);
        Shuffle(monsterSpawnPoints);
        
        for (int i = 0; i < count; i++)
        {
            Transform pos = monsterSpawnPoints[i];
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
        };
        
        livingEntity.onDead += handler;
        monsters.Add((livingEntity, handler));
    }


    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private FieldObjectData GetRandomItem()
    {
        int rand = Random.Range(0, totalPb);
        int cumulative = 0;

        for (int i = 0; i < spawnItems.Count; i++)
        {
            cumulative += spawnItems[i].Probability;
            if (rand < cumulative)
            {
                return spawnItems[i].FieldObject;
            }
        }
        
        return null;
    }


    private void DestroyObject()
    {
        //리스트 내부 전부 풀로 되돌리고 리스트 초기화
        for (int i = 0; i < currentObjects.Count; i++)
        {
            currentObjects[i].ReleasePool();
        }
        currentObjects.Clear();

        
        
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

    [ContextMenu("DebugRockCount")]
    public void DebugRockCount()
    {
        Debug.Log($"{spawnPointsParent.transform.childCount}");
    }
    
    
    [ContextMenu("DebugMonsterCount")]
    public void DebugMonsterCount()
    {
        Debug.Log($"{monsterSpawnPointsParent.transform.childCount}");
    }
}
