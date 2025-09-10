using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class ConsumeItemData : CountableItemData
{
    [SerializeField] protected List<StatModifier> statModifier;
    
    
    public List<StatModifier> GetStatModifier()
    {
        if (statModifier.Count == 0)
        {
            
            return null;
        }
        
        return statModifier;
    }
    
}
