using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "BombDebuff", menuName = "SO/Debuff/Bomb")]
public class BombDebuffBase : DebuffBase
{

    [SerializeField] private List<BombLevelAmount> bombLevelAmounts;
    
    
    public override WeaponBuffer CreateDebuff()
    {
        return new BombBuffer(this, debuffLevel);
    }

    public List<BombLevelAmount> BombLevelAmounts => bombLevelAmounts;
}

[System.Serializable]
public class BombLevelAmount
{
    public float afterBombTime;
    public float damage;
}