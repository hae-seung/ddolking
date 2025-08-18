using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ThirdAttack : ScriptableObject
{
    [SerializeField] protected int id;
    [SerializeField] protected GameObject thirdAttackPb;
    [SerializeField] protected float attackDamage;

    protected Transform spawnPos;
    
    public void Execute(BossAI boss)
    {
        Debug.Log("3타 추가타 발생");
        UseSkill(boss);
    }

    protected abstract void UseSkill(BossAI boss);
}
