using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipItem : Item, IStatModifier
{
    public EquipItemData EquipData { get; private set; }
    protected List<StatModifier> statModifiers = null;
    private ItemEnhancementLogic itemEnhancementLogic = null;

    public int curLevel { get; private set; }
    protected float curDurability;
    protected float maxDurability;

    public float CurDurability => curDurability;
    public float MaxDurability => maxDurability;
    
    public ItemEnhancementLogic GetLogic => itemEnhancementLogic;
    
    
    protected EquipItem(EquipItemData data) : base(data)
    {
        EquipData = data;
        curLevel = data.itemLevel;
        maxDurability = data.maxDurability;
        curDurability = data.maxDurability;
        
        if (data.GetStatModifier() != null)
            statModifiers = new List<StatModifier>(data.GetStatModifier());
        
        if (data.isEnhanceable)
            itemEnhancementLogic = data.GetLogic();
    }

    public List<StatModifier> GetStatModifier()
    {
        return statModifiers;
    }
    
    public EquipItem Clone()
    {
        return CreateItem();
    }
    
    protected abstract EquipItem CreateItem();

    
    //어차피 아뮬렛은 강화시스템에 못들어가게 막아두었음.
    public void LevelUp()
    {
        if (curLevel >= 5)
            return;

        //내구도 증가
        UpgradeDurability();
        
        curLevel++;

        if (itemEnhancementLogic == null)
            return;

        var enhanceLogics = itemEnhancementLogic.GetItemEnhanceLogic();
        var nextLogic = enhanceLogics.Find(logic => logic.nextLevel == curLevel);

        if (nextLogic == null)
            return;

        foreach (var enhancement in nextLogic.statEnhancements)
        {
            // 기존 스탯이 있는지 확인
            var existing = statModifiers.Find(mod => mod.stat == enhancement.stat);

            if (existing != null)
            {
                existing.increaseAmount += enhancement.enhanceValue;
            }
            else
            {
                statModifiers.Add(new StatModifier (enhancement.stat, enhancement.enhanceValue));
            }

            if (GameEventsManager.Instance.playerEvents.GetHandItem() == this)
            {
                GameEventsManager.Instance.statusEvents.AddStat(enhancement.stat, enhancement.enhanceValue);
            }
        }
    }

    private void UpgradeDurability()
    {
        switch (EquipData.itemclass)
        {
            case ItemClass.Normal:
                maxDurability += 30;
                curDurability = Mathf.Clamp(curDurability + 30, 0, maxDurability);
                break;
            case ItemClass.Epic:
                maxDurability += 50;
                curDurability = Mathf.Clamp(curDurability + 50, 0, maxDurability);
                break;
            case ItemClass.Unique:
                maxDurability += 80;
                curDurability = Mathf.Clamp(curDurability + 80, 0, maxDurability);
                break;
            case ItemClass.Legend:
                maxDurability += 100;
                curDurability = Mathf.Clamp(curDurability + 100, 0, maxDurability);
                break;
        }
    }

    public void RepairDurability()
    {
        curDurability = Mathf.Clamp(curDurability + 10, 0, maxDurability);
    }
}
