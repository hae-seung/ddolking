using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DebuffBase : ScriptableObject
{
    [Header("디버프 레벨")] 
    [SerializeField] public int debuffLevel = 1;
    
    [Header("데미지 이펙트")]
    [SerializeField] public DebuffType debuffType;
    
    [Header("데미지스킨")]
    [SerializeField] public DamageType damageType;
    
    [Header("디버프 적용 확률")]
    [SerializeField] public int debuffProbability;
    
    

    public abstract WeaponBuffer CreateDebuff();

}
