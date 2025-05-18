using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public abstract class WeaponItemData : EquipItemData
{
    //무기는 스킬도 가짐
    [Header("공격마리수 : 단일/다수")]
    [SerializeField] protected int hitCount;

    [Header("공격횟수 / 몇타")] 
    [SerializeField] protected int attackCount;

    [Header("무기상수 / 단일 : 0.1~1.0 / 다수 : 1.0~")] 
    [SerializeField] protected float weaponConst;

    [Header("사거리")] 
    [SerializeField] protected float range;

    [Header("공격딜레이 / 기본 1초")] 
    [SerializeField] protected float attackDelay;
    
    [Header("검사할 레이어 / LivingEntity")]
    [SerializeField] protected LayerMask targetLayer;
    protected string targetTag = "LivingEntity";

    [Header("상태이상")] 
    [SerializeField] protected DebuffBase debuff;


    public float AttackDelay => attackDelay;
    
    public override Item CreateItem()
    {
        return new WeaponItem(this);
    }


    public abstract void ExecuteAttack(Vector2 dir, Vector2 origin, WeaponItem weapon = null);
    
    protected float CalculateDamage(bool isCritical)
    {
        return GameEventsManager.Instance.calculatorEvents.CalculatePlayerAttackDamage(weaponConst, isCritical);
    }

}
