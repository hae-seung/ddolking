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
    [SerializeField] private List<Transform> spawnPoints;

    [Header("스폰 아이템과 확률")] 
    [SerializeField] private List<SpawnProbability> spawnItems;


    private List<SpawnObject> currentObjects;
    
    private int totalPb;

    private void Start()
    {
        currentObjects = new List<SpawnObject>();
        for(int i = 0; i < spawnItems.Count; i++)
        {
            totalPb += spawnItems[i].Probability;
            ObjectPoolManager.Instance.RegisterPrefab(spawnItems[i].FieldObject.id, 
                spawnItems[i].FieldObject.ownObject);
        }
        
        
    }

    public void Spawn()
    {
        DestroyObject();
        
        int spawnCount = Random.Range(spawnPoints.Count - 10, spawnPoints.Count +1);
        spawnCount = Mathf.Clamp(spawnCount, 0, spawnPoints.Count + 1); 
        Shuffle();
        
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
    }


    private void Shuffle()
    {
        for (int i = spawnPoints.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (spawnPoints[i], spawnPoints[j]) = (spawnPoints[j], spawnPoints[i]);
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
    }
}
