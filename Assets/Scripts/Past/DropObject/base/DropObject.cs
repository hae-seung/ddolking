using System.Collections;
using UnityEngine;

public abstract class DropObject : MonoBehaviour
{
    protected int itemId;
    private Collider2D _collider;
    private float disableCollisionTime = 0.5f;
    private float autoReleaseTime = 5f;  // 1분 후 자동 반환 시간
    protected bool IsFirstAwake = false;

    protected virtual void Awake()
    {
        _collider = GetComponent<Collider2D>();
        IsFirstAwake = true;
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(DisableCollisionTemporarily());

        // 1분 후 자동으로 풀에 반환
        Invoke(nameof(AutoReleaseToPool), autoReleaseTime);
    }

    protected virtual void OnDisable()
    {
        // 비활성화되면 자동 반환 취소
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
        // 풀 등록 확인 후 자동 등록
        if (!ObjectPoolManager.Instance.IsPoolRegistered(itemId))
        {
            ObjectPoolManager.Instance.RegisterPrefab(itemId, gameObject);
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

    public abstract int GetItemId();
}
