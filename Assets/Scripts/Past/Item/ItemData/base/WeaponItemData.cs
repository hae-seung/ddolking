
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

    [Header("상태이상")] 
    [SerializeField] protected DebuffBase debuff;


    [Header("내구도가 닳지 않아야 하는 무기일 경우 체크")]
    [SerializeField] public bool isHandWeapon = false;
    
    
    
    public int HitCount => hitCount;
    public int AttackCount => attackCount;
    public float WeaponConst => weaponConst;
    public float Range => range;
    public float AttackDelay => attackDelay;
    public LayerMask TargetLayer => targetLayer;
    public DebuffBase Debuff => debuff;
    
    
    private string targetTag = "LivingEntity";
    
}
