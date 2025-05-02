using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public abstract class LivingEntity : MonoBehaviour
{
    //스텟
    protected float hp;
    protected float toolWear;
    protected float defense;



    protected NavMeshAgent agent;
    
    public event Action<float> onDamage;
    public event Action onDead;


    protected virtual EntityData EntityData => null;
    public bool IsDead { get; private set; }


    protected virtual void Awake()
    {
        hp = EntityData.Hp;
        toolWear = EntityData.ToolWear;
    }
    
    
    public virtual void StopMove()
    {
        agent.isStopped = true;
    }

    
    
    public virtual void MoveRandom()
    {
        
    }
    
    
    
    
    public virtual void OnDamage(float damage)
    {
        onDamage?.Invoke(damage);
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
