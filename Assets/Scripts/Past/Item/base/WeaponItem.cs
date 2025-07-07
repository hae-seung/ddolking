using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponItem : EquipItem, IDurabilityReduceable
{
    private WeaponItemData data;
    
    protected int hitCount;
    protected int attackCount;
    private float weaponConst;
    protected float range;
    private float attackDelay;
    protected LayerMask targetLayer;
    protected string targetTag = "LivingEntity";
    protected DebuffBase debuff;
    protected WeaponBuffer weaponBuffer;

    
    
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

        if (debuff != null)
        {
            weaponBuffer = debuff.CreateDebuff();
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
        //비어있음.
        //Line, RangeWeapon에서 각각 오버라이드 사용
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
