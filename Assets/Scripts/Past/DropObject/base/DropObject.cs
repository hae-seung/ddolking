using System.Collections;
using UnityEngine;

public abstract class DropObject : MonoBehaviour
{
    protected int itemId;
    protected GameObject dropObjectPrefab;
    private Collider2D _collider;
    private float disableCollisionTime = 0.5f;
    private float autoReleaseTime = 60f;  // 1분 후 자동 반환 시간
    protected bool isSpawned = false;  // 풀에서 가져왔거나 인스턴스화된 경우

    protected virtual void Awake()
    {
        _collider = GetComponent<Collider2D>();
        isSpawned = false;  // 기본적으로 씬에 배치된 상태
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(DisableCollisionTemporarily());

        // 풀에서 가져온 경우(SpawnObject를 통해), 자동 반환 설정
        if (isSpawned)
        {
            Invoke(nameof(AutoReleaseToPool), autoReleaseTime);
        }
    }

    protected virtual void OnDisable()
    {
        // 비활성화 시 자동 반환 취소
        CancelInvoke(nameof(AutoReleaseToPool));
    }

    private IEnumerator DisableCollisionTemporarily()
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(disableCollisionTime);
        _collider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HandleItemPickup();
        }
    }

    private void HandleItemPickup()
    {
        if (!ObjectPoolManager.Instance.IsPoolRegistered(itemId))
        {
            ObjectPoolManager.Instance.RegisterPrefab(itemId, dropObjectPrefab);
        }

        CollectItem();
    }

    protected abstract void CollectItem();

    private void AutoReleaseToPool()
    {
        if (ObjectPoolManager.Instance.IsPoolRegistered(itemId))
        {
            Debug.Log($"오브젝트 자동 반환: {gameObject.name}, ID: {itemId}");
            ObjectPoolManager.Instance.ReleaseObject(itemId, gameObject);
        }
        else
        {
            Debug.LogError($"{gameObject.name}: 풀에 등록되지 않아 파괴됨.");
            Destroy(gameObject);
        }
    }

    public void DestroyDropObject()
    {
        CancelInvoke(nameof(AutoReleaseToPool));  // 수동 반환 시 자동 반환 취소

        if (ObjectPoolManager.Instance.IsPoolRegistered(itemId))
        {
            Debug.Log($"오브젝트 반환 시 ID 확인: {gameObject.name}, ID: {itemId}");
            ObjectPoolManager.Instance.ReleaseObject(itemId, gameObject);
        }
        else
        {
            Debug.LogError($"{gameObject.name}: DropObject에 itemId가 설정되지 않음.");
            Destroy(gameObject);
        }
    }

    // 풀에서 나온 경우 호출 (스폰될 때 실행)
    public void SetAsSpawned()
    {
        isSpawned = true;
    }

    public abstract int GetItemId();
}
