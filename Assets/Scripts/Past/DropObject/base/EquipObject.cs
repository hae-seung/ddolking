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

    protected override void CollectItem()
    {
        int amount = Inventory.Instance.Add(equipItem);
        if (amount <= 0)
            Destroy(gameObject); //장비 아이템은 몇 없으므로 풀에 등록하지 않음
    }

    protected abstract void MakeItemInstance();
    
}
