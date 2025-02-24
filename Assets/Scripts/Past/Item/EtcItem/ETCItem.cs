using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETCItem : CountableItem
{
    public EtcItemData EtcData { get; private set; }
    
    public ETCItem(EtcItemData data, int amount = 1) : base(data, amount)
    {
        EtcData = data;
    }

    protected override CountableItem CreateItem()
    {
        return new ETCItem(CountableData as EtcItemData);
    }
}
