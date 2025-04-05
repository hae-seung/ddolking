using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipObject<T> : DropObject where T : EquipItem
{
    protected T equipItem;

    protected override void Awake()
    {
        MakeItemInstance();
        base.Awake();
    }

    protected override void OnEnable()
    {
        if (!isSpawned)  // 씬에 배치된 경우 == 최초 활성화 시 작업 방지
        {
            isSpawned = true;
            return;
        }
        
        MakeItemInstance();
        base.OnEnable();
    }

    protected override void CollectItem()
    {
        if (!ObjectPoolManager.Instance.IsPoolRegistered(itemData.ID))
        {
            ObjectPoolManager.Instance.RegisterPrefab(itemData.ID, dropObjectPrefab);
        }
        
        int amount = Inventory.Instance.Add(equipItem);
        if (amount <= 0)
            DestroyDropObject();
    }

    protected abstract void MakeItemInstance();
    
}
