using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ThirdAttack", menuName = "Entity/Boss/ThirdAttack/MossKnight")]
public class MossKnightThirdAttack : ThirdAttack
{
    [SerializeField] private float waitTime;
    [SerializeField] private float fallSpeed;
    [SerializeField] private float destroyDelay;
    
    
    
    protected override void UseSkill(BossAI boss)
    {
        if(!ObjectPoolManager.Instance.IsPoolRegistered(id))
            ObjectPoolManager.Instance.RegisterPrefab(id, thirdAttackPb);

        Vector3 targetPos = boss.target.transform.position;
        Vector3 pos = boss.target.transform.position + Vector3.up * 10f;

        BossThirdAttacker attacker = ObjectPoolManager.Instance.SpawnObject(
            id,
            pos,
            Quaternion.identity).GetComponent<BossThirdAttacker>();
        
        attacker.Init(waitTime, destroyDelay, attackDamage, boss, targetPos, id);
        attacker.SetFallSpeed(fallSpeed);
        attacker.StartRoutine();
    }
}
