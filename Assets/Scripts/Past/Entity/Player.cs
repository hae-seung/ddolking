using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator a;
    
    private Color playerHalfA = new Color(1, 1, 1, 0.5f);
    private Color fullA = new Color(1, 1, 1, 1);
    private bool isHurt = false;
    private WaitForSeconds hurtTime = new WaitForSeconds(0.5f);


    private bool isDead;
    
    private void Awake()
    {
        GameEventsManager.Instance.playerEvents.onDead += Dead;
    }

    private void OnEnable()
    {
        isHurt = false;
    }

    private void Dead()
    {
        isDead = true;
        //사망시 처리
        //사망 UI => 부활 누르면 SpawnPoint에서 부활
        //소지골드 절반 날아가기
        //Time.scale = 0f;

    }

    public void OnDamage(float damage)
    {
        if (isDead)
            return;
        
        float applyDamage = GameEventsManager.Instance.calculatorEvents.CalculateDamage(damage);
        Debug.Log(applyDamage);
        GameEventsManager.Instance.statusEvents.AddStat(Stat.HP, -applyDamage);

        // if (isDead)
        //     return;
        
        StartCoroutine(HurtRoutine());
        StartCoroutine(AlphaBlink());
    }

    private IEnumerator AlphaBlink()
    {
        while (isHurt)
        {
            yield return new WaitForSeconds(0.1f);
            sr.color = playerHalfA;
            yield return new WaitForSeconds(0.1f);
            sr.color = fullA;
        }
    }

    private IEnumerator HurtRoutine()
    {
        isHurt = true;
        yield return hurtTime;
        isHurt = false;
    }
}
