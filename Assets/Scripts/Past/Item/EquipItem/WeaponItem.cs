using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : EquipItem
{
    private WeaponItemData data;
    //스킬저장
    public WeaponItem(WeaponItemData data) : base(data)
    {
        this.data = data;
    }

    protected override EquipItem CreateItem()
    {
        return new WeaponItem(EquipData as WeaponItemData);
    }
}
