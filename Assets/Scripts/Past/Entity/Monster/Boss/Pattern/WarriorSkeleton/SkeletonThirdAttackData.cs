using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "ThirdAttack", menuName = "Entity/Boss/ThirdAttack/WarriorSkeleton")]
public class SkeletonThirdAttackData : ThirdAttack
{
    [SerializeField] private float waitTime;
    [SerializeField] private float flySpeed;

    [SerializeField] private int spawnSwordId;
    [SerializeField] private GameObject swordBullet;
    
    protected override void UseSkill(BossAI boss)
    {
        if(!ObjectPoolManager.Instance.IsPoolRegistered(id))
            ObjectPoolManager.Instance.RegisterPrefab(id, thirdAttackPb);
        
        if(!ObjectPoolManager.Instance.IsPoolRegistered(spawnSwordId))
            ObjectPoolManager.Instance.RegisterPrefab(spawnSwordId, swordBullet);
        
        
        //발사대
        WarriorSkeletonThirdAttacker attacker = ObjectPoolManager.Instance.SpawnObject(
            id,
            boss.transform.position,
            Quaternion.identity).GetComponent<WarriorSkeletonThirdAttacker>();

        attacker.transform.parent = boss.transform;

        attacker.Init(id, attackDamage, waitTime, flySpeed, boss, spawnSwordId);
        attacker.StartRoutine();
    }
}
