using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionItem : CountableItem
{
    public PortionItem(PortionItemData data, int amount = 1) : base(data, amount){ }

    public bool Use()
    {
        Amount--;
        return true;
    }

    
}
