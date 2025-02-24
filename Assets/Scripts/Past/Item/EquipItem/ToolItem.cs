using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolItem : EquipItem
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
}
