using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponItem : WeaponItem
{
    private RangeWeaponData data;
    private float fanAngle;
    
    private Collider2D[] hitBuffer = new Collider2D[20];
    
    
    
    public RangeWeaponItem(RangeWeaponData data) : base(data)
    {
        this.data = data;
        fanAngle = data.FanAngle;
    }

    protected override EquipItem CreateItem()
    {
        return new RangeWeaponItem(EquipData as RangeWeaponData);
    }
    
    public override void ExecuteAttack(Vector2 dir, Vector2 origin)
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
                
                dmg?.OnDamage(CalculateDamage(isCritical), isCritical);
            }
            
            if(dmg != null)
                ReduceDurability(dmg.GetToolWear());
            
            if (weaponBuffer != null && weaponBuffer.CanApply())
                dmg?.ApplyDebuff(weaponBuffer);
            
            validCount++;
            
            if (validCount >= hitCount)
                break;
        }
    }
    
    
}
