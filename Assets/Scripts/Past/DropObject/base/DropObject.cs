using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public abstract class DropObject : MonoBehaviour
{
    protected int itemId;  // 고유 아이템 ID (상속받은 클래스에서 설정)
    
    private Collider2D _collider;
    private float disableCollisionTime = 0.5f;
    
    protected bool IsFirstAwake = false;

    protected virtual void Awake()
    {
        _collider = GetComponent<Collider2D>();
        RegisterPool();
        IsFirstAwake = true;
    }
    
    protected virtual void OnEnable()
    {
        StartCoroutine(DisableCollisionTemporarily());
    }

    protected void RegisterPool()
    {
        // 이미 풀에 등록되었는지 확인 후 등록
        if (!ObjectPoolManager.Instance.IsPoolRegistered(itemId))
        {
            ObjectPoolManager.Instance.CreatePool(itemId, gameObject);
            Debug.Log($"{gameObject.name} (ID: {itemId}) 풀 등록 완료");
        }
        else
        {
            Debug.Log($"{gameObject.name} (ID: {itemId}) 이미 풀에 등록됨");
        }
    }


    public void DestroyDropObject()
    {
        if (itemId > 0)
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

    protected IEnumerator DisableCollisionTemporarily()
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(disableCollisionTime);
        _collider.enabled = true;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            AddItemToInventory();
    }

    protected abstract void AddItemToInventory();

    public int GetItemId()
    {
        return itemId;
    }
}