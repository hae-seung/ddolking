using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmuletItemData", menuName = "SO/EquipItemData/Amulet")]
public class AmuletItemData : EquipItemData
{
    
    public override Item CreateItem()
    {
        return new AmuletItem(this);
    }
    
    
    private void OnValidate()
    {
#if UNITY_EDITOR
        maxDurability = 1; //아뮬렛은 내구도가 1고정
        isEnhanceable = false;
        itemLevel = 0;
#endif
    }
}
