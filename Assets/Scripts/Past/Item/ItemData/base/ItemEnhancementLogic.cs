using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StatEnhanceData
{
    public Stat stat; // 강화할 스탯 (예: MaxHP, Str)
    public EnhanceFormula enhanceFormula; // 강화 방식 (Fixed, Proportional, Exponential)
    public float enhanceValue; // 증가량 (예: +50, +10%, x1.1)
}

[System.Serializable]
public struct ItemEnhanceLogic
{
    public int nextLevel; // 다음 레벨
    public int needGold; // 필요 골드
    public List<StatEnhanceData> statEnhancements; // 해당 레벨에서 강화될 스탯 리스트
}

public enum EnhanceFormula
{
    Fixed,         // 일정 수치 증가
    Proportional,  // % 증가
    Exponential    // 지수 증가
}


[CreateAssetMenu(fileName = "ItemEnhanceLogic", menuName = "SO/EquipItemData/EnhanceLogic", order = int.MinValue)]
public class ItemEnhancementLogic : ScriptableObject
{
    [SerializeField] private List<ItemEnhanceLogic> _itemEnhanceLogic;

    public List<ItemEnhanceLogic> GetItemEnhanceLogic()
    {
        return _itemEnhanceLogic;
    }
}

