using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DropObjectManager : Singleton<DropObjectManager>
{
    private Dictionary<GameObject, ObjectPool<DropObject>> dropObjectPools = new();

    public ObjectPool<DropObject> GetOrCreatePool(GameObject dropPrefab)
    {
        if (!dropObjectPools.ContainsKey(dropPrefab))
        {
            // 먼저 풀을 생성하고, 이후에 CreateDropObject에서 참조할 수 있도록 처리
            var pool = new ObjectPool<DropObject>(
                () => CreateDropObject(dropPrefab),
                OnGetDropObject,
                OnReleaseDropObject,
                OnDestroyDropObject,
                false, 10, 30
            );
            dropObjectPools[dropPrefab] = pool; // 생성된 풀을 먼저 저장
        }

        return dropObjectPools[dropPrefab]; // 저장된 풀 반환
    }

    private DropObject CreateDropObject(GameObject dropPrefab)
    {
        // 여기서 dropObjectPools[dropPrefab] 접근 시 문제 발생을 방지
        DropObject newItem = Instantiate(dropPrefab).GetComponent<DropObject>();

        // 이미 풀을 생성한 후이므로 dropObjectPools[dropPrefab]가 유효
        newItem.SetManagedPool(dropObjectPools[dropPrefab]);
        return newItem;
    }

    private void OnGetDropObject(DropObject item)
    {
        item.gameObject.SetActive(true);
    }

    private void OnReleaseDropObject(DropObject item)
    {
        item.gameObject.SetActive(false);
    }

    private void OnDestroyDropObject(DropObject item)
    {
        Destroy(item.gameObject);
    }
}