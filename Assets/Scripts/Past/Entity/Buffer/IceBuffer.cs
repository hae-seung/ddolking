using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IceBuffer : WeaponBuffer
{
    private IceDebuffBase data;
    private List<IceLevelAmount> iceLevelAmounts;

    public IceBuffer(IceDebuffBase data, int level) : base(data, level)
    {
        this.data = data;
        iceLevelAmounts = data.IceLevelAmounts;
    }

    protected override IEnumerator OnDebuffStart(IDamageable entity, UnityAction onEnd)
    {
        if (!(entity is LivingEntity monster))
        {
            yield break;
        }

        // 속도 고정
        monster.OverrideAgentSpeed();

        // 지속 시간만큼 대기
        yield return new WaitForSeconds(iceLevelAmounts[debuffLevel - 1].duration);

        // 속도 복원
        monster.ReleaseAgentSpeed();

        // 디버프 종료 처리
        onEnd?.Invoke();
    }

    public override WeaponBuffer CreateBuffer(int level)
    {
        return new IceBuffer(data, level);
    }

    public override string GetDebuffDescription()
    {
        string description = $"{iceLevelAmounts[debuffLevel - 1].duration}동안 적의 움직임을 제한합니다";
        return description;
    }

    public override string GetNextDebuffDescription()
    {
        string description = $"{iceLevelAmounts[debuffLevel].duration}동안 적의 움직임을 제한합니다";
        return description;
    }
}