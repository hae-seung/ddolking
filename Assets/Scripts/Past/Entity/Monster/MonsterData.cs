using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MonsterData", menuName = "SO/Entity/Monster")]
public class MonsterData : EntityData
{
    [Header("원거리/근거리")]
    [SerializeField] private bool isAdCarry;
    
    [Header("목표속도 도달 시간 = 목표속도 / 가속도")]
    [SerializeField] private float runSpeed;
    [SerializeField] private float acc;
    
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackDamage;
    [SerializeField] private float bodyDamage;

    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;

    [Header("레벨별 증가량/ 보스는 0")] 
    [SerializeField] private float hpRatio;
    [SerializeField] private float damageRatio;
    [SerializeField] private float defenseRatio;
    [SerializeField] private float toolWearRatio;

    
    public bool IsAdCarry => isAdCarry;
    public float RunSpeed => runSpeed;
    public float Acc => acc;
    public float AttackDelay => attackDelay;
    public float AttackDamage => attackDamage;
    public float BodyDamage => bodyDamage;
    public float SightRange => sightRange;
    public float AttackRange => attackRange;

    public float HpRatio => hpRatio;
    public float DamageRatio => damageRatio;
    public float DefenseRatio => defenseRatio;
    public float ToolWearRatio => toolWearRatio;
}
