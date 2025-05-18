using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "RangeWeapon", menuName = "SO/EquipItemData/Weapon/Range")]
public class RangeWeaponData : WeaponItemData
{
    
    [Header("범위")] [Range(0, 360)]
    [SerializeField] private float fanAngle;


    private Collider2D[] hitBuffer = new Collider2D[20];
    
    public override void ExecuteAttack(Vector2 dir, Vector2 origin, WeaponItem weapon = null)
    {
        int hitCountActual = Physics2D.OverlapCircleNonAlloc(origin, range, hitBuffer, targetLayer);

        int validCount = 0;
        
        int criticalNum = (int)GameEventsManager.Instance.statusEvents.GetStatValue(Stat.Critical);
        bool isCritical = false;
        
        
        for (int i = 0; i < hitCountActual; i++)
        {
            Transform target = hitBuffer[i].transform;

            if (!target.CompareTag(targetTag))
                continue;

            Vector2 toTarget = ((Vector2)target.position - origin).normalized;
            float angle = Vector2.Angle(dir, toTarget);
            if (angle > fanAngle * 0.5)
                continue;

            
            var dmg = target.GetComponent<IDamageable>();
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
            
            validCount++;
            if (validCount >= hitCount)
                break;
        }
    }
}
