using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BombBuffer : WeaponBuffer
{
    private BombDebuffBase data;
    private List<BombLevelAmount> bombLevelAmounts;

    public BombBuffer(BombDebuffBase data, int level) : base(data, level)
    {
        this.data = data;
        bombLevelAmounts = data.BombLevelAmounts;
    }

    protected override IEnumerator OnDebuffStart(IDamageable entity, UnityAction onEnd)
    {
        float delayTime = bombLevelAmounts[debuffLevel - 1].afterBombTime;
        yield return new WaitForSeconds(delayTime);

        entity.OnDebuffDamage(data.damageType, data.debuffType, bombLevelAmounts[debuffLevel - 1].damage);
        onEnd?.Invoke();
    }
    

    public override WeaponBuffer CreateBuffer(int level)
    {
        return new BombBuffer(data, level);
    }

    public override string GetDebuffDescription()
    {
        string description = $"{bombLevelAmounts[debuffLevel - 1].afterBombTime} 후에 " +
                             $"{bombLevelAmounts[debuffLevel - 1].damage} 만큼의 1회 폭발 데미지를 고정으로 입힙니다.";

        return description;
    }

    public override string GetNextDebuffDescription()
    {
        string description = $"{bombLevelAmounts[debuffLevel].afterBombTime} 후에 " +
                             $"{bombLevelAmounts[debuffLevel].damage} 만큼의 1회 폭발 데미지를 고정으로 입힙니다.";

        return description;
    }
}