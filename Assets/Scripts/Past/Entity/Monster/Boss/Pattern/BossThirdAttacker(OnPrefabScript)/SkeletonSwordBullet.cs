using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSwordBullet : MonoBehaviour
{
    private int id;
    private float speed;
    private float damage;

    private Coroutine launchRoutine;
    
    public void Init(int id, float speed, float damage)
    {
        this.id = id;
        this.speed = speed;
        this.damage = damage;
        
        if(launchRoutine != null)
            StopCoroutine(launchRoutine);
    }

    public void Launch()
    {
        if(gameObject.activeSelf)
            launchRoutine = StartCoroutine(StartLaunch());
    }

    private IEnumerator StartLaunch()
    {
        float timer = 0f;
        while (timer < 3f)
        {
            transform.position += transform.up * speed * Time.deltaTime; 
            timer += Time.deltaTime;
            yield return null;
        }
        
        ObjectPoolManager.Instance.ReleaseObject(id, gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player target = other.GetComponent<Player>();

            if (target == null)
                return;

            if (target.OnDamage(damage))
            {
                if(launchRoutine != null)
                    StopCoroutine(launchRoutine);
            
                ObjectPoolManager.Instance.ReleaseObject(id, gameObject);
            }
        }
    }
}
