using System.Collections;
using System.Collections.Generic;

public abstract class EquipItem : Item, IStatModifier
{
    public EquipItemData EquipData { get; private set; }
    protected List<StatModifier> statModifiers = null;
    private ItemEnhancementLogic itemEnhancementLogic = null;

    protected int curLevel;
    protected float curDurability;
    private int curDurabilityPower = 0;
    //내구력 노말 : 3%, 에픽 : 5%, 유니크 : 7%, 레전더리 : 10% 고정값, 3렙강화 부터 무조건 활성화
    
    
    protected EquipItem(EquipItemData data) : base(data)
    {
        EquipData = data;
        curLevel = data.itemLevel;
        curDurability = data.maxDurability;
        
        if (data.GetStatModifier() != null)
            statModifiers = new List<StatModifier>(data.GetStatModifier());
        
        if (data.isEnhanceable)
            itemEnhancementLogic = data.GetLogic();
        
        SetDurabilityPower(curLevel);
    }

    private void SetDurabilityPower(int curLevel)
    {
        if (curLevel < 3)
            return;

        int durabilityValue = (int)EquipData.itemclass;
        int amount = curLevel - EquipData.itemLevel;
        curDurability = durabilityValue + (amount * durabilityValue);
    }

    public List<StatModifier> GetStatModifier()
    {
        return statModifiers;
    }
}
