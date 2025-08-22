using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "ThirdAttack", menuName = "Entity/Boss/ThirdAttack/Reaper")]
public class ReaperThirdAttackData : ThirdAttack
{
    [SerializeField] private int id2;
    [SerializeField] private GameObject thirdAttackPb2;
    
    
    protected override void UseSkill(BossAI boss)
    {
        RegisterPool();
        SpawnPrefab(id, thirdAttackPb, boss);
        SpawnPrefab(id2, thirdAttackPb2, boss);
    }


    private void SpawnPrefab(int id, GameObject thirdAttack, BossAI boss)
    {
        Quaternion rot = thirdAttack.transform.rotation;
        Vector3 scale = thirdAttack.transform.localScale; 
        
        
        ReaperBossSweepThirdAttacker attacker = ObjectPoolManager.Instance.SpawnObject(
            id,
            boss.transform.position,
            Quaternion.identity).GetComponent<ReaperBossSweepThirdAttacker>();

        attacker.Init(id, attackDamage, boss);
        
        attacker.transform.SetParent(boss.transform, false);

        attacker.transform.position = boss.transform.position;
        attacker.transform.rotation = rot;
        attacker.transform.localScale = scale;
    }
    
    
    
    
    private void RegisterPool()
    {
        if (!ObjectPoolManager.Instance.IsPoolRegistered(id))
        {
            ObjectPoolManager.Instance.RegisterPrefab(id, thirdAttackPb);
        }
        
        if (!ObjectPoolManager.Instance.IsPoolRegistered(id2))
        {
            ObjectPoolManager.Instance.RegisterPrefab(id2, thirdAttackPb2);
        }
    }
}
