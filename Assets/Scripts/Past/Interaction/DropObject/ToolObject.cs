using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolObject : EquipObject<ToolItem>
{
    [SerializeField] private ToolItemData data;
    
    protected override void MakeItemInstance()
    {
        equipItem = new ToolItem(data);
        itemData = data;
    }
    
    
}
