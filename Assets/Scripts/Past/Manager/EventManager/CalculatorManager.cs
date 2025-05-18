using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CalculatorManager : MonoBehaviour
{
    private void Awake()
    {
        GameEventsManager.Instance.calculatorEvents.onCalculateMineDamage += CalculateMineDamage;
        GameEventsManager.Instance.calculatorEvents.onCalculateDamage += CalculateDamage;
        GameEventsManager.Instance.calculatorEvents.onCalculatePlayerAttackDamage += CalculatePlayerAttackDamage;
    }
    
    //플레이어가 입히는 데미지 계산
    private float CalculatePlayerAttackDamage(float weaponConst, bool isCritical)
    {
        float baseDamage = GameEventsManager.Instance.statusEvents.GetStatValue(Stat.Str);
        float damage = baseDamage * weaponConst;

        // 다단히트를 위한 데미지 변동: ±10% 범위 (예: 0.9 ~ 1.1배)
        float variance = Random.Range(0.9f, 1.1f);
        damage *= variance;

        if (isCritical)
            damage *= 2f;

        return damage;
    }


    //플레이어가 입는 데미지 계산
    private float CalculateDamage(float damage)
    {
        float defense = GameEventsManager.Instance.statusEvents.GetStatValue(Stat.Defense);
        float realDamage = damage * (1 - defense);
        return realDamage;
    }


    private float CalculateMineDamage(Stat targetStat)
    {
        return GameEventsManager.Instance.statusEvents.GetStatValue(targetStat);
    }
}
