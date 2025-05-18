using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DebuffBase : ScriptableObject
{
    public string debuffId;
    public int debuffProbability;

    public abstract IEnumerator ApplyEffect(IDamageable damageable);

    public bool CanApply()
    {
        int ran = Random.Range(1, 101); // 1 이상 100 이하
        return debuffProbability >= ran;
    }

}
