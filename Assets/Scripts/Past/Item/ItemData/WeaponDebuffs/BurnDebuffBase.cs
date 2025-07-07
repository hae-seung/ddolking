using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.Timeline;


[CreateAssetMenu(fileName = "BurnDebuff", menuName = "SO/Debuff/Burn")]
public class BurnDebuffBase : DebuffBase
{
    [Header("레벨별 증가량")] 
    [SerializeField] private List<BurnLevelAmount> burnLevelAmounts;
    
    public override WeaponBuffer CreateDebuff()
    {
        return new BurnBuffer(this, debuffLevel);
    }

    public List<BurnLevelAmount> BurnLevelAmounts => burnLevelAmounts;
}


[System.Serializable]
public class BurnLevelAmount
{
    public float damagePerTick;
    public float duration;
    public float interval;
}
