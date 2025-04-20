using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItemData", menuName = "SO/EquipItemData/Weapon")]
public class WeaponItemData : EquipItemData
{
    
    //무기는 스킬도 가짐
    
    public override Item CreateItem()
    {
        return new WeaponItem(this);
    }
}
