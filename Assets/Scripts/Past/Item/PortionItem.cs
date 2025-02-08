using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionItem : ConsumeItem
{
    public PortionItemData PortionData { get; private set; }

    public PortionItem(PortionItemData data, int amount = 1) : base(data, amount)
    {
        PortionData = data;
    }
    

    protected override CountableItem CreateItem()
    {
        return new PortionItem(CountableData as PortionItemData);
    }


    protected override bool UseItem()
    {
        Amount--;
        return true;
    }
}
