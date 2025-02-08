using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstablishItem : CountableItem, IUseable
{
    public EstablishItemData EstablishData { get; private set; }
    
    protected EstablishItem(EstablishItemData data, int amount = 1) : base(data, amount)
    {
        EstablishData = data;
    }

    protected override CountableItem CreateItem()
    {
        return new EstablishItem(CountableData as EstablishItemData);
    }

    public bool Use()
    {
        //todo:설치작업
        return true;
    }
}
