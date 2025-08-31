using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkill : MonoBehaviour
{

    protected SkillData data;
    public abstract void ActiveSkill();
    public abstract void Init(SkillData data);
    
    protected float CalculateDamage(bool isCritical)
    {
        return GameEventsManager.Instance.calculatorEvents.CalculatePlayerAttackDamage(data.SkillConst, isCritical);
    }
}
