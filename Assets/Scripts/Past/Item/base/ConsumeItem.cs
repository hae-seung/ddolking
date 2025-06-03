
using System.Collections.Generic;
using UnityEngine;


public abstract class ConsumeItem : CountableItem, IUseable, IStatModifier
{
    public ConsumeItemData ConsumeData { get; private set; }
    protected List<StatModifier> statModifiers;
    
    public ConsumeItem(ConsumeItemData data, int amount = 1) : base(data, amount)
    {
        ConsumeData = data;
        statModifiers = data.GetStatModifier();
    }
    
    public bool Use()
    {
        return UseItem();
    }

    public List<StatModifier> GetStatModifier()
    {
        return statModifiers;
    }

    protected abstract bool UseItem();
}
