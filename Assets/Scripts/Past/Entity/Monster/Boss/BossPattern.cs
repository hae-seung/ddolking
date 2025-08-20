using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BossPattern : ScriptableObject
{
    [SerializeField] protected float cooldown;
    [SerializeField] protected float damage;
    
    public float Cooldown => cooldown;
    
    

    public void UseSkill(BossAI boss)
    {
        ExecutePattern(boss);
    }

    
    
    protected abstract void ExecutePattern(BossAI boss);
}
