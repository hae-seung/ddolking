using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdCarryMonster : Monster
{
    [Header("MonsterAttack 필요없음")]
    [Header("발사 물체")]
    [SerializeField] private ProjectileData projectileData;
    [SerializeField] private Transform launchPos;

    [Header("발사 이펙트")] 
    [SerializeField] private bool hasLaunch;
    [SerializeField] private int launchId;
    [SerializeField] private GameObject launchPrefab;
    
    protected override void Awake()
    {
        base.Awake();

        if (!ObjectPoolManager.Instance.IsPoolRegistered(projectileData.Id))
        {
            ObjectPoolManager.Instance.RegisterPrefab(projectileData.Id, projectileData.Prefab);
        }

        if (hasLaunch && !ObjectPoolManager.Instance.IsPoolRegistered(launchId))
        {
            ObjectPoolManager.Instance.RegisterPrefab(launchId, launchPrefab);
        }
    }
    
    
    public override void PerformAttack()
    {
        Projectile projectile = ObjectPoolManager.Instance.SpawnObject(
            projectileData.Id,
            new Vector3(launchPos.position.x, launchPos.position.y, 0), //z 축 수정 필요
            Quaternion.identity).GetComponent<Projectile>();

        projectile.SetUp(attackDamage, target, projectileData.Id);
        
        lastAttackTime = Time.time;

        SetLaunch();
    }

    private void SetLaunch()
    {
        if (!hasLaunch)
            return;

        ObjectPoolManager.Instance.SpawnObject(
            launchId,
            launchPos.position,
            Quaternion.identity);
    }
}
