using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossAnimationReceiever : MonoBehaviour
{
    [SerializeField] private BossAI bossAI;
        
    public UnityEvent onFootstep;
    public UnityEvent onAttack;
    public UnityEvent onDieFx;
        
    public void OnFootstep()
    {
        onFootstep?.Invoke();
    }

    public void OnAttack()
    {
        onAttack?.Invoke();
    }

    public void OnDieFx()
    {
        onDieFx?.Invoke();
    }
        
        
    public void OnAnimationEnd()
    {
        bossAI.OnAnimationEnd();
    }
}
