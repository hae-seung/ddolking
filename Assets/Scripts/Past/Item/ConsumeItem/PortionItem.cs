using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionItem : ConsumeItem
{
    public PortionItemData PortionData { get; private set; }
    private int experienceAmount;
    

    public PortionItem(PortionItemData data, int amount = 1) : base(data, amount)
    {
        PortionData = data;
        experienceAmount = data.ExperienceAmount;
    }
    

    protected override CountableItem CreateItem()
    {
        return new PortionItem(CountableData as PortionItemData);
    }


    protected override bool UseItem()
    {
        if (statModifiers != null)//버프물약
        {
            for (int i = 0; i < statModifiers.Count; i++)
            {
                GameEventsManager.Instance.statusEvents.
                    AddStat(statModifiers[i].stat, statModifiers[i].increaseAmount);
            }
        }
        else//경험치 물약
        {
            GameEventsManager.Instance.playerEvents.GainExperience(experienceAmount);
        }
        
        Amount--;
        return true;
    }
}
