using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReaperBossSweepThirdAttacker : MonoBehaviour
{
    private float damage;
    private BossAI boss;
    private int id;
    
    public void Init(int id, float damage, BossAI boss)
    {
        this.id = id;
        this.damage = damage;
        this.boss = boss; 
    }


    public void HitParticle()
    {
        boss.target.OnDamage(damage);
    }

    public void ReleasePool()
    {
        ObjectPoolManager.Instance.ReleaseObject(id, gameObject);
    }
    
}
