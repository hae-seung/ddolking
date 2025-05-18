using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatorEvents
{
    public event Func<Stat, float> onCalculateMineDamage;
    public float CalculateMineDamage(Stat stat)
    {
        float mineDamage = onCalculateMineDamage.Invoke(stat);
        return mineDamage;
    }

    
    
    /// <summary>
    /// 플레이어가 피격시 입는 피해량 계산
    /// </summary>
    public event Func<float, float> onCalculateDamage;
    public float CalculateDamage(float damage)
    {
        float takeDamage = onCalculateDamage(damage);
        return takeDamage;
    }

    /// <summary>
    /// 플레이어가 입히는 피해량 계산
    /// </summary>
    public event Func<float, bool, float> onCalculatePlayerAttackDamage;
    public float CalculatePlayerAttackDamage(float weaponConst, bool isCritical)
    {
        float attackDamage = onCalculatePlayerAttackDamage(weaponConst, isCritical);
        return attackDamage;
    }

}
