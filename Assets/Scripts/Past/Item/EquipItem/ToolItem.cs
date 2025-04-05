using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolItem : EquipItem, IDurabilityReduceable
{
    private ToolItemData data;
    
    public ToolItem(ToolItemData data) : base(data)
    {
        this.data = data;
    }

    protected override EquipItem CreateItem()
    {
        return new ToolItem(EquipData as ToolItemData);
    }


    public void ReduceDurability(float amount)
    {
        curDurability -= amount;
        if (curDurability <= 0)
        {
            Inventory.Instance.RemoveItem(this);
        }
    }
}
