using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MossKnightThirdAttacker : MonoBehaviour
{
    private float fallSpeed;
    
    private float waitTime;
    private float destroyDelay;


    private int id;
    private float damage;
    private Vector3 des;
    private Player target;
    protected BossAI boss;
    
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
    
    public void StartRoutine()
    {
        StartCoroutine(FallRoutine());
    }
    
    private IEnumerator FallRoutine()
    {
        yield return new WaitForSeconds(waitTime);
        
        while (Vector3.Distance(transform.position, des) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, des, 
                fallSpeed * Time.deltaTime);
            yield return null;
        }
        
        yield return new WaitForSeconds(destroyDelay);
        ObjectPoolManager.Instance.ReleaseObject(id, gameObject);
    }
    
    private void SetDes(Vector3 des)
    {
        Vector3 newDes = des + Vector3.down * 30f;
        this.des = newDes;
    }

    public void SetFallSpeed(float speed)
    {
        fallSpeed = speed;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target.OnDamage(damage);
        }
    }
    
}
