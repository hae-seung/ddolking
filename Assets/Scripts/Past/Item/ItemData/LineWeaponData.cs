using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LineWeapon", menuName = "SO/EquipItemData/Weapon/Line")]
public class LineWeaponData : WeaponItemData
{
    private RaycastHit2D[] cachedHits = new RaycastHit2D[10]; // 초기 크기
    private HashSet<Transform> uniqueTargets = new();
    private List<Transform> validTargets = new List<Transform>();

    public override void ExecuteAttack(Vector2 dir, Vector2 origin, WeaponItem weapon = null)
    {
        // hitCount에 따라 배열 크기 조절
        if (cachedHits.Length < hitCount)
            cachedHits = new RaycastHit2D[hitCount];

        uniqueTargets.Clear();
        validTargets.Clear();

        int hitLength = Physics2D.RaycastNonAlloc(origin, dir, cachedHits, range, targetLayer);
        
        
        int count = 0;
        for (int i = 0; i < hitLength; i++)
        {
            var hit = cachedHits[i];
            if (!hit.transform.CompareTag(targetTag))
                continue;

            if (uniqueTargets.Add(hit.transform))
            {
                validTargets.Add(hit.transform);
                count++;
            }

            if (count >= hitCount)
                break;
        }


        int criticalNum = (int)GameEventsManager.Instance.statusEvents.GetStatValue(Stat.Critical);
        bool isCritical = false;
        
        for (int i = 0; i < validTargets.Count; i++)
        {
            var dmg = validTargets[i].GetComponent<IDamageable>();

            for (int j = 0; j < attackCount; j++)
            {
                isCritical = false;
                int ran = Random.Range(1, 101);
                if (criticalNum >= ran)
                    isCritical = true;
                    
                dmg?.OnDamage(CalculateDamage(isCritical), isCritical, weapon);
            }

            if (debuff != null && debuff.CanApply())
                dmg?.ApplyDebuff(debuff);
        }
    }
    
}