using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmuletObject : EquipObject<AmuletItem>
{
    [SerializeField] private AmuletItemData data;
    
    
    protected override void MakeItemInstance()
    {
        equipItem = new AmuletItem(data);
        itemData = data;
    }
    
    
}
