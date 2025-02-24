using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETCObject : CountableObject<ETCItem>
{
    [SerializeField] private EtcItemData data;
    
    protected override void MakeItemInstance()
    {
        countableItem = new ETCItem(data);
        itemData = data;
    }
    
}
