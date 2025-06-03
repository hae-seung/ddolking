using System;
using System.Collections;
using UnityEngine;

public class DropObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    private Item item;
    private GameObject dropObjectPrefab;
    private Collider2D _collider;
    private float disableCollisionTime = 0.5f;
    

    private void Awake()//인스턴스 생성 후 base.Awake로 마지막에 실행됨.
    {
        _collider = GetComponentInChildren<Collider2D>();
        dropObjectPrefab = itemData.DropObjectPrefab;
    }

    private void OnEnable()
    {
        item = itemData.CreateItem();
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


    private void CollectItem()
    {
        if(!ObjectPoolManager.Instance.IsPoolRegistered(itemData.ID))
            ObjectPoolManager.Instance.RegisterPrefab(itemData.ID, dropObjectPrefab);
        
        if (item is CountableItem citem)
        {
            int amountLeftAfterAdd = Inventory.Instance.Add(citem, citem.Amount);

            if (amountLeftAfterAdd > 0)
            {
                citem.SetAmount(amountLeftAfterAdd);
            }
            else
            {
                DestroyDropObject();
            }
        }
        else
        {
            int amount = Inventory.Instance.Add(item);
            if (amount <= 0)
                DestroyDropObject();
        }
    }
    
    
    private void DestroyDropObject()
    {
        ObjectPoolManager.Instance.ReleaseObject(itemData.ID, gameObject);
    }
}
