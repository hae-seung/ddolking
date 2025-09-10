using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmuletItem : EquipItem
{
    private AmuletItemData data;
    
    public AmuletItem(AmuletItemData data) : base(data)
    {
        this.data = data;
    }

    public void EquipAmulet()
    {
        if (statModifiers != null)
        {
            for (int i = 0; i < statModifiers.Count; i++)
            {
                GameEventsManager.Instance.statusEvents.
                    AddStat(statModifiers[i].stat, statModifiers[i].increaseAmount);
            }
        }
    }

    public void UnEquipAmulet()
    {
        if (statModifiers != null)
        {
            for (int i = 0; i < statModifiers.Count; i++)
            {
                GameEventsManager.Instance.statusEvents.
                    AddStat(statModifiers[i].stat, -statModifiers[i].increaseAmount);
            }
        }
    }


    public void OverrideStatModifier(List<StatModifier> statModifier)
    {
        statModifiers = new List<StatModifier>(statModifier);
    }
    
    protected override EquipItem CreateItem()
    {
        return new AmuletItem(EquipData as AmuletItemData);
    }
    
    
    
}
