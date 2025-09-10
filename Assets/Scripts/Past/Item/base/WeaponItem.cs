using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponItem : EquipItem, IDurabilityReduceable
{
    public WeaponItemData data;
    
    protected int hitCount;
    protected int attackCount;
    private float weaponConst;
    protected float range;
    private float attackDelay;
    protected LayerMask targetLayer;
    protected string targetTag = "LivingEntity";
    private DebuffBase debuff;
    protected WeaponBuffer weaponBuffer;


    private int tempHitCount;
    private float tempRange;
    
    public float AttackDelay => attackDelay;
    
    //스킬저장
    public WeaponItem(WeaponItemData data) : base(data)
    {
        this.data = data;
        
        hitCount = data.HitCount;
        attackCount = data.AttackCount;
        weaponConst = data.WeaponConst;
        range = data.Range;
        attackDelay = data.AttackDelay;
        targetLayer = data.TargetLayer;
        debuff = data.Debuff;

        tempRange = range;
        tempHitCount = hitCount;
        
        if (debuff != null)
        {
            weaponBuffer = debuff.CreateDebuff();
        }
    }

    public override void LevelUp()
    {
        base.LevelUp();
        if (weaponBuffer != null)
        {
            if (curLevel == 3 || curLevel == 5)
            {
                weaponBuffer.LevelUp();
            }
        }
    }
    
    public void ReduceDurability(float amount)
    {
        //내구력% 계산하여 내구력 감소 시키기
        if (data.isHandWeapon)
            return;
    }

    public virtual void ExecuteAttack(Vector2 dir, Vector2 origin)
    {
        range = tempRange + GameEventsManager.Instance.statusEvents.GetStatValue(Stat.AttackRange);
        hitCount = tempHitCount + (int)GameEventsManager.Instance.statusEvents.GetStatValue(Stat.AdditionalAttackCount);
    }
    
    protected float CalculateDamage(bool isCritical)
    {
        return GameEventsManager.Instance.calculatorEvents.CalculatePlayerAttackDamage(weaponConst, isCritical);
    }

    public string GetDebuffDescription()
    {
        if (weaponBuffer != null)
        {
            return weaponBuffer.GetDebuffDescription();
        }

        return "";
    }

    public string GetNextDebuffDescription()
    {
        if (weaponBuffer != null)
        {
            return weaponBuffer.GetNextDebuffDescription();
        }

        return "";
    }
    
    
    
}
