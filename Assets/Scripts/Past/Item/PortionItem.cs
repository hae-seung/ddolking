using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionItem : CountableItem
{
    public PortionItem(PortionItemData data, int amount = 1) : base(data, amount){ }
    

    protected override CountableItem CreateItem(int amount)
    {
        return new PortionItem(CountableData as PortionItemData, amount);
    }
    
}
