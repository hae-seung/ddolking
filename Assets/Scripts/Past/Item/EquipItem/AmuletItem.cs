using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmuletItem : EquipItem
{
    private AmuletItemData data;
    
    public AmuletItem(AmuletItemData data) : base(data)
    {
        this.data = data;
    }
    
}
