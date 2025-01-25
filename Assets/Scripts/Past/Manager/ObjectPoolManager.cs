using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    private Dictionary<int, ObjectPool<GameObject>> objectPools = new();
    private Dictionary<int, GameObject> registeredPrefabs = new();
    private const int maxPoolSize = 30;  // 최대 풀 크기 설정

    public void CreatePool(int id, GameObject prefab, int initialSize = 10)
    {
        if (objectPools.ContainsKey(id))
        {
            Debug.LogWarning($"풀 ID {id}가 이미 등록되어 있습니다.");
            return;
        }

        registeredPrefabs[id] = prefab;

        var pool = new ObjectPool<GameObject>(
            () => CreateObject(id),
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyObject,
            false, initialSize, maxPoolSize
        );

        objectPools[id] = pool;
        Debug.Log($"풀 생성 완료: ID {id}");
    }

    public bool IsPoolRegistered(int id)
    {
        return objectPools.ContainsKey(id);
    }

    public GameObject SpawnObject(int id, Vector3 position, Quaternion rotation)
    {
        if (!objectPools.ContainsKey(id))
        {
            Debug.LogWarning($"풀 ID {id}가 존재하지 않아 새로 생성합니다.");
            return InstantiateAndReturn(id, position, rotation);
        }

        ObjectPool<GameObject> pool = objectPools[id];

        // 풀의 maxSize 초과 시, 새로운 오브젝트 인스턴스화하여 리턴
        if (pool.CountActive >= maxPoolSize)
        {
            Debug.LogWarning($"풀 ID {id}가 최대 크기 {maxPoolSize}에 도달하여 새 오브젝트를 인스턴스화합니다.");
            return InstantiateAndReturn(id, position, rotation);
        }

        GameObject obj = pool.Get();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        return obj;
    }

    public void ReleaseObject(int id, GameObject obj)
    {
        if (objectPools.ContainsKey(id))
        {
            ObjectPool<GameObject> pool = objectPools[id];

            // 현재 풀의 최대 크기를 초과할 경우 오브젝트 삭제
            if (pool.CountInactive >= maxPoolSize)
            {
                Debug.LogWarning($"풀 ID {id}가 최대 크기 초과로 오브젝트 삭제됨: {obj.name}");
                Destroy(obj);
                return;
            }

            Debug.Log($"풀 ID {id}에 오브젝트 반환: {obj.name}");
            pool.Release(obj);
        }
        else
        {
            Debug.LogWarning($"풀 ID {id}가 존재하지 않아 오브젝트를 직접 파괴합니다: {obj.name}");
            Destroy(obj);
        }
    }


    private GameObject InstantiateAndReturn(int id, Vector3 position, Quaternion rotation)
    {
        if (!registeredPrefabs.ContainsKey(id))
        {
            Debug.LogError($"ID {id}에 대한 프리팹이 등록되지 않았습니다.");
            return null;
        }

        GameObject newObj = Instantiate(registeredPrefabs[id], position, rotation);
        newObj.SetActive(true);
        return newObj;
    }

    private GameObject CreateObject(int id)
    {
        if (!registeredPrefabs.ContainsKey(id))
        {
            Debug.LogError($"ID {id}에 대한 프리팹이 등록되지 않았습니다.");
            return null;
        }

        GameObject newObj = Instantiate(registeredPrefabs[id]);
        newObj.SetActive(false);
        newObj.transform.SetParent(Instance.transform);
        return newObj;
    }

    private void OnGetFromPool(GameObject obj)
    {
        obj.SetActive(true);
        obj.transform.SetParent(null);
    }

    private void OnReleaseToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(Instance.transform);
    }

    private void OnDestroyObject(GameObject obj)
    {
        Destroy(obj);
    }
}
