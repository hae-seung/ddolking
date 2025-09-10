using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LineWeapon", menuName = "SO/EquipItemData/Weapon/Line")]
public class LineWeaponData : WeaponItemData
{
    public override Item CreateItem()
    {
        return new LineWeaponItem(this);
    }
}