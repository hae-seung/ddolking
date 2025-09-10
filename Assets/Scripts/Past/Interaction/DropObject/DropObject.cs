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
    

    private void Awake()
    {
        _collider = GetComponentInChildren<Collider2D>();
        dropObjectPrefab = itemData.DropObjectPrefab;
    }

    private void OnEnable()
    {
        if(item == null)
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
        if (item == null)
            return;
        
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
                item = null;
                DestroyDropObject();
            }
        }
        else//장비 아이템은 추가되면 바로 추가
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

    
    
    /// <summary>
    /// 기존에 존재하던(정보를 유지해야하는) 아이템인경우
    /// </summary>
    /// <param name="item"></param>
    public void OverrideItem(EstablishItem item)
    {
        this.item = item;
        item.SetAmount(1);
    }
    
}
