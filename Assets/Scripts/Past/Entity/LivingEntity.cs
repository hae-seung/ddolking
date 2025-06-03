using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using DamageNumbersPro;
using Random = UnityEngine.Random;
using Slider = UnityEngine.UI.Slider;


public interface IDamageable
{
    public void OnDamage(float damage, bool isCritical,DebuffType debuff ,WeaponItem weaponItem = null);
    public void ApplyDebuff(DebuffBase debuff);
}



public abstract class LivingEntity : MonoBehaviour, IDamageable
{
    [SerializeField] protected Slider healthSlider;
    
    [Header("성장스텟")]
    protected float hp;
    protected float toolWear;
    protected float defense;
    
    
    protected NavMeshAgent agent;
    
    public event Action<float> onDamage;
    public event Action onDead;


    protected virtual EntityData EntityData => null;
    public bool IsDead { get; private set; }
    
    
    private HashSet<string> activeDebuffIDs = new HashSet<string>();


    protected virtual void Awake()
    {
        hp = EntityData.Hp;
        toolWear = EntityData.ToolWear;
        
        healthSlider.maxValue = hp;
        healthSlider.value = hp;
    }

    protected void OnEnable()
    {
        //몬스터의 최대 체력이 늘어날 수 있음.
        healthSlider.maxValue = hp;
        healthSlider.value = hp;
    }


    public virtual void StopMove()
    {
        agent.isStopped = true;
        agent.speed = 0f;
    }

    
    
    public virtual void MoveRandom()
    {
        
    }
    
    
    public virtual void OnDamage(float damage, bool isCritical, DebuffType debuff, WeaponItem weaponItem = null)
    {
        if (debuff != DebuffType.none)
        {
            //todo : 디버프로 인한 피해 적용 로직 구상
            return;
        }
        
        
        //데미지 효과 및 스킨
        ApplyDamageEffect(damage, isCritical);
            
        
        //피해 애니메이션 및 Stop적용
        onDamage?.Invoke(damage);
        
        
        damage *= (1 - defense);
        hp -= damage;
        
        //체력바 적용
        healthSlider.value -= damage;
        
        //무기 내구도 적용
        if(weaponItem != null)
            weaponItem.ReduceDurability(toolWear);
        
        if (hp <= 0)
        {
            OnDead();
        }
    }

    private void ApplyDamageEffect(float damage, bool isCritical)
    {
        //데미지 스킨
        DamageNumber damageNumber
            = DamageManager.Instance.GetDamageSkin(isCritical ? DamageType.critical : DamageType.normal);

        damageNumber.Spawn(transform.position, damage);
        damageNumber.SetFollowedTarget(transform);
        
        
        //히트 이펙트
        if (isCritical)
        {
            ObjectPoolManager.Instance.SpawnObject(
                (int)DamageType.critical,
                transform.position,
                Quaternion.identity);
        }
        else
        {
            ObjectPoolManager.Instance.SpawnObject(
                (int)DamageType.normal,
                transform.position,
                Quaternion.identity);
        }
    }


    public void ApplyDebuff(DebuffBase debuff)
    {
        if (activeDebuffIDs.Contains(debuff.debuffId))
        {
            return;
        }
        
        activeDebuffIDs.Add(debuff.debuffId);
        StartCoroutine(RunDebuff(debuff));
    }

    private IEnumerator RunDebuff(DebuffBase debuff)
    {
        yield return debuff.ApplyEffect(this);

        RemoveDebuff(debuff.debuffId);
    }

    private void RemoveDebuff(string debuffID)
    {
        activeDebuffIDs.Remove(debuffID);
    }


    protected void OnDead()
    {
        onDead?.Invoke();
    }

    
    public void DisableObject()
    {
        int entityId = EntityData.EntityId;
        
        DropItem(EntityData);
        
        //오브젝트 풀에 반환
        if (!ObjectPoolManager.Instance.IsPoolRegistered(entityId))
        {
            ObjectPoolManager.Instance.RegisterPrefab(entityId, gameObject);
        }
        
        ObjectPoolManager.Instance.ReleaseObject(entityId, gameObject);
        
    }
    
    private void DropItem(EntityData entityData)
    {
        GameEventsManager.Instance.playerEvents.GainExperience(entityData.DropTable.experience);
        
        
        for (int i = 0; i < entityData.DropTable.items.Count; i++)
        {
            int amount = entityData.DropTable.items[i].CalculateAndGetPrefab();
            for (int j = 0; j < amount; j++)
            {
                if(!ObjectPoolManager.Instance.IsPoolRegistered(entityData.DropTable.items[i].dropItem.ID))
                    ObjectPoolManager.Instance.RegisterPrefab(
                        entityData.DropTable.items[i].dropItem.ID, 
                        entityData.DropTable.items[i].dropItem.DropObjectPrefab);
                
                Vector3 dropPosition = transform.position + new 
                    Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.2f, 0.2f), 0);
                
                GameObject dropObj = ObjectPoolManager.Instance.SpawnObject(
                    entityData.DropTable.items[i].dropItem.ID, 
                    dropPosition, 
                    Quaternion.identity);
                
                dropObj?.transform.DOJump(dropPosition, 0.5f, 1, 0.8f).SetEase(Ease.OutBounce);
            }
        }
    }

    
}
