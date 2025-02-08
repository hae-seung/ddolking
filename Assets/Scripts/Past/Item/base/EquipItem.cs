using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipItem : Item
{
    public EquipItemData EquipData { get; private set; }
    
    protected EquipItem(EquipItemData data) : base(data)
    {
        EquipData = data;
    }
}
