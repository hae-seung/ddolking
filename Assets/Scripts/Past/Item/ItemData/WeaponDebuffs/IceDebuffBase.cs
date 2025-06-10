using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "IceDebuff", menuName = "SO/Debuff/Ice")]
public class IceDebuffBase : DebuffBase
{
    [Header("빙결 지속 설정")]
    public float duration = 5f;

    [SerializeField] private List<IceLevelAmount> iceLevelAmounts;
    
    public override WeaponBuffer CreateDebuff()
    {
        return new IceBuffer(this, debuffLevel);
    }

    public List<IceLevelAmount> IceLevelAmounts => iceLevelAmounts;
}

[Serializable]
public class IceLevelAmount
{
    public float duration;
}