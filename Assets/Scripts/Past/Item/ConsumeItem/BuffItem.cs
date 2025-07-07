using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffItem : ConsumeItem
{
    public BuffItemData BuffData { get; private set; }
    
    public BuffItem(BuffItemData data, int amount = 1) : base(data, amount)
    {
        BuffData = data;
    }

    protected override CountableItem CreateItem()
    {
        return new BuffItem(CountableData as BuffItemData);
    }

    protected override bool UseItem()
    {
        if (GameEventsManager.Instance.playerEvents.ApplyPortionBuff(this))
        {
            Amount--;
            return true;
        }
        
        
        return false;
    }
}
