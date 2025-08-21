using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "ShieldPattern", menuName = "Entity/Boss/Pattern/Shield")]
public class SkeletonShieldPattern : BossPattern
{
    [SerializeField] private int shieldId;
    [SerializeField] private GameObject shieldPrefab;
    
    [SerializeField] private float shieldHp;
    [SerializeField] private float defense;
    [SerializeField] private float toolWear;
    
    
    protected override void ExecutePattern(BossAI boss)
    {
        if(!ObjectPoolManager.Instance.IsPoolRegistered(shieldId))
            ObjectPoolManager.Instance.RegisterPrefab(shieldId, shieldPrefab);

        Shield shield = ObjectPoolManager.Instance.SpawnObject(
            shieldId,
            boss.transform.position,
            Quaternion.identity).GetComponent<Shield>();

        shield.transform.SetParent(boss.transform, false);

        shield.transform.position = boss.transform.position;
        
        
        shield.Init(shieldId, shieldHp, boss, defense, toolWear);
    }
}
