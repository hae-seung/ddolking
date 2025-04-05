using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : EquipObject<WeaponItem>
{
    [SerializeField] private WeaponItemData data;
    
    
    protected override void MakeItemInstance()
    {
        equipItem = new WeaponItem(data);
        itemData = data;
    }
    
    
}
