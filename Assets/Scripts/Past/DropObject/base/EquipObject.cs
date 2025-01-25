using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipObject : DropObject
{
    [SerializeField]
    //protected  itemData;
    protected CountableItem countableItem;

    protected override void Awake()
    {
        MakeItemInstance();
        base.Awake();
    }

    protected abstract void MakeItemInstance();
    protected override void AddItemToInventory()
    {
        
    }
}
