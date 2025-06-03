using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffItem : ConsumeItem
{
    private int duration;
    
    public BuffItemData BuffData { get; private set; }
    
    public BuffItem(BuffItemData data, int amount = 1) : base(data, amount)
    {
        BuffData = data;
        duration = data.BuffDuration;
    }

    protected override CountableItem CreateItem()
    {
        return new BuffItem(CountableData as BuffItemData);
    }

    protected override bool UseItem()
    {
        GameEventsManager.Instance.playerEvents.ApplyPortionBuff(statModifiers, duration);
        Amount--;
        return true;
    }
}
