using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    [Header("풀 기본 설정")]
    public int defaultCapacity = 10;
    public int maxPoolSize = 50;

    private Dictionary<int, IObjectPool<GameObject>> pools = new();
    private Dictionary<int, GameObject> registeredPrefabs = new();

    // 프리팹 등록 및 풀 생성
    public void RegisterPrefab(int id, GameObject prefab)
    {
        if (pools.ContainsKey(id))
        {
            Debug.Log($"ID {id}는 이미 등록된 프리팹입니다.");
            return;
        }

        registeredPrefabs[id] = prefab;
        pools[id] = new ObjectPool<GameObject>(
            () => CreatePooledItem(id),
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            false, defaultCapacity, maxPoolSize
        );

        Debug.Log($"ID {id}의 프리팹이 풀에 등록되었습니다.");
    }

    // 아이템 생성 (Instantiate)
    private GameObject CreatePooledItem(int id)
    {
        if (!registeredPrefabs.ContainsKey(id))
        {
            Debug.LogError($"ID {id}의 프리팹이 등록되지 않았습니다!");
            return null;
        }

        GameObject obj = Instantiate(registeredPrefabs[id]);
        obj.SetActive(false);
        return obj;
    }

    // 오브젝트 풀에서 꺼내기
    private void OnTakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }

    // 오브젝트 풀에 반환
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
        poolGo.transform.SetParent(transform);
    }

    // 오브젝트 제거
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }

    // 아이템 스폰 (필드에서 드롭 시)
    public GameObject SpawnObject(int id, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (!pools.ContainsKey(id))
        {
            Debug.LogWarning($"ID {id}의 풀을 찾을 수 없습니다. 새로운 오브젝트를 생성합니다.");
            RegisterPrefab(id, registeredPrefabs[id]);
        }

        GameObject obj = pools[id].Get();
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        if (parent != null)
        {
            obj.transform.SetParent(parent);
        }
        else
        {
            obj.transform.SetParent(null);
        }

        // DropObject의 SetAsSpawned() 호출
        obj.GetComponent<DropObject>()?.SetAsSpawned();

        Debug.Log($"ID {id}의 오브젝트 풀에서 가져옴: {obj.name}");
        return obj;
    }


    // 아이템 반환 (플레이어가 획득할 때)
    public void ReleaseObject(int id, GameObject obj)
    {
        if (!pools.ContainsKey(id))
        {
            Debug.LogWarning($"ID {id}의 풀을 찾을 수 없습니다. 오브젝트를 삭제합니다.");
            Destroy(obj);
            return;
        }

        pools[id].Release(obj);
    }

    // 특정 ID에 대한 풀이 등록되어 있는지 확인
    public bool IsPoolRegistered(int id)
    {
        return pools.ContainsKey(id);
    }
}
