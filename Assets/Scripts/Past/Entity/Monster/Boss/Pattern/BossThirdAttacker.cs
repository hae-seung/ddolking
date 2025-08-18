using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossThirdAttacker : MonoBehaviour
{
     protected float waitTime;
     protected float destroyDelay;


    protected int id;
    protected float damage;
    protected Vector3 des;
    protected Player target;
    protected BossAI boss;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target.OnDamage(damage);
        }
    }

    public void Init(float waitTime, float destroyDelay, float thirdDamage, BossAI boss, Vector3 des, int id)
    {
        this.id = id;
        SetTime(waitTime, destroyDelay);
        SetDamage(thirdDamage, boss);
        SetDes(des);
    }

    private void SetTime(float waitTime, float destroyDelay)
    {
        this.waitTime = waitTime;
        this.destroyDelay = destroyDelay;
    }


    private void SetDamage(float thirdDamage, BossAI boss)
    {
        damage = thirdDamage;
        this.boss = boss;
        target = boss.target;
    }

    
    //des는 일반적으로 플레이어 위치
    protected virtual void SetDes(Vector3 des){ }
    public virtual void SetFallSpeed(float fallSpeed){ }
    public abstract void StartRoutine();

}
