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
            Quaternion.identity,
            boss.transform).GetComponent<Shield>();
        
        shield.transform.position = boss.transform.position;
        Vector3 scale = new Vector3(1.5f, 1.5f, 1f);
        shield.transform.localScale = scale;
        
        shield.Init(shieldId, shieldHp, boss, defense, toolWear);
    }
}
