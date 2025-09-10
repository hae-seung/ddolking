using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GrowItemData", menuName = "SO/CountableItemData/ETC/GrowItemData")]
public class GrowItemData : EtcItemData, IGrowable
{
    [Header("최대성장감소시간")]
    [Range(1f, 100f)]
    [SerializeField] private float reduceTime;

    [Header("성장타입 : seed와 animal중 선택")] 
    [SerializeField] private DamageType growType;
    
    
    public float GetMaxGrowTime()
    {
        return reduceTime;
    }

    public bool GetTypeCorrect(DamageType type)
    {
        return type == growType;
    }
}
