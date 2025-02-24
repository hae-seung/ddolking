using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstablishObject : CountableObject<EstablishItem>
{
    [SerializeField] private EstablishItemData data;
    
    protected override void MakeItemInstance()
    {
        countableItem = new EstablishItem(data);
        itemData = data;
    }
}
