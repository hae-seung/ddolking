using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstablishItem : CountableItem
{
    public EstablishItemData EstablishData { get; private set; }
    
    public EstablishItem(EstablishItemData data, int amount = 1) : base(data, amount)
    {
        EstablishData = data;
    }

    protected override CountableItem CreateItem()
    {
        return new EstablishItem(CountableData as EstablishItemData);
    }

    public void Use()
    {
        Amount--;
    }
   
}
