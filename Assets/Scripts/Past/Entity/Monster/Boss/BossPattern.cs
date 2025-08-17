using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BossPattern : ScriptableObject
{
    [SerializeField] protected float cooldown;
    private float lastUsedTime;

    protected Action EndSkill;

    public bool IsReady()
    {
        return Time.time - lastUsedTime >= cooldown;
    }

    public void UseSkill(BossAI boss, Action onEndSkill)
    {
        lastUsedTime = Time.time;
        EndSkill = onEndSkill;
        ExecutePattern(boss);
    }
    
    protected abstract void ExecutePattern(BossAI boss);
}
