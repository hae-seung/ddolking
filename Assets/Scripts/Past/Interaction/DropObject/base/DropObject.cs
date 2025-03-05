using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class DropObject : MonoBehaviour
{
    protected ItemData itemData;
    protected GameObject dropObjectPrefab;
    private Collider2D _collider;
    private float disableCollisionTime = 0.5f;
    
    protected bool isSpawned = false;  // 풀에서 가져왔거나 인스턴스화된 경우

    protected virtual void Awake()//인스턴스 생성 후 base.Awake로 마지막에 실행됨.
    {
        _collider = GetComponentInChildren<Collider2D>();
        isSpawned = false;  // 기본적으로 씬에 배치된 상태
        dropObjectPrefab = itemData.DropObjectPrefab;
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(DisableCollisionTemporarily());
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
            CollectItem();
        }
    }
    

    protected abstract void CollectItem();
    
    
    protected void DestroyDropObject()
    {
        if (ObjectPoolManager.Instance.IsPoolRegistered(itemData.ID))
        {
            ObjectPoolManager.Instance.ReleaseObject(itemData.ID, gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
