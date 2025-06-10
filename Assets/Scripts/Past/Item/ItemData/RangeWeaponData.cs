using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "RangeWeapon", menuName = "SO/EquipItemData/Weapon/Range")]
public class RangeWeaponData : WeaponItemData
{
    [Header("범위")] [Range(0, 360)]
    [SerializeField] private float fanAngle;

    public float FanAngle => fanAngle;
    
    
    public override Item CreateItem()
    {
        return new RangeWeaponItem(this);
    }
    
    
}
