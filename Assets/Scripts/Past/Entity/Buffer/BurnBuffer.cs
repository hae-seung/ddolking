using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BurnBuffer : WeaponBuffer
{
    private BurnDebuffBase data;
    private List<BurnLevelAmount> burnLevelAmounts;
    
    public BurnBuffer(BurnDebuffBase data, int level) : base(data, level)
    {
        this.data = data;
        burnLevelAmounts = data.BurnLevelAmounts;
        //level은 부모가 알아서
    }

    protected override IEnumerator OnDebuffStart(IDamageable target, UnityAction onEnd)
    {
        float elapsed = 0f;
        var entity = target as LivingEntity;

        while (elapsed < burnLevelAmounts[debuffLevel - 1].duration)
        {
            if (entity == null || entity.IsDead)
                break;

            target.OnDebuffDamage(data.damageType, data.debuffType, burnLevelAmounts[debuffLevel - 1].damagePerTick);
            yield return new WaitForSeconds(data.BurnLevelAmounts[debuffLevel - 1].interval);
            elapsed += burnLevelAmounts[debuffLevel - 1].interval;
        }

        onEnd?.Invoke();
    }

    public override WeaponBuffer CreateBuffer(int level)
    {
        return new BurnBuffer(data, level);
    }

    public override string GetDebuffDescription()
    {
        string description = $"{burnLevelAmounts[debuffLevel - 1].duration}초 동안" +
                             $" {burnLevelAmounts[debuffLevel - 1].damagePerTick}만큼의 고정 화상 데미지를" +
                             $" {burnLevelAmounts[debuffLevel - 1].damagePerTick}초 간격으로 입힙니다.";
        return description;
    }

    public override string GetNextDebuffDescription()
    {
        string description = $"{burnLevelAmounts[debuffLevel].duration}초 동안" +
                             $" {burnLevelAmounts[debuffLevel].damagePerTick}만큼의 고정 화상 데미지를" +
                             $" {burnLevelAmounts[debuffLevel].damagePerTick}초 간격으로 입힙니다.";
        return description;
    }
}
