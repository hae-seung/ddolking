using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AmuletItemData", menuName = "SO/EquipItemData/Amulet")]
public class AmuletItemData : EquipItemData
{
    [SerializeField] private List<AmuletEffect> _amuletEffects;
    
    
    
    private void OnValidate()
    {
#if UNITY_EDITOR
        maxDurability = 1; //아뮬렛은 내구도가 1고정
        isEnhanceable = false;
        itemLevel = 0;
#endif
    }
}
