using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using DamageNumbersPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Slider = UnityEngine.UI.Slider;

public interface IDamageable
{
    void OnDamage(float damage, bool isCritical);
    void OnDebuffDamage(DamageType damageType, DebuffType debuffType, float damage);
    float GetToolWear();
    void ApplyDebuff(WeaponBuffer buffer);
}


public interface IDebuffable
{
    public void OnEndDebuff();
}

public abstract class LivingEntity : MonoBehaviour, IDamageable, IDebuffable
{
    [SerializeField] protected Slider healthSlider;
    [SerializeField] protected Image debuffIcon;

    [Header("성장스텟")]
    protected float hp;
    protected float toolWear;
    protected float defense;

    protected NavMeshAgent agent;
    public bool IsDead { get; private set; }

    public event Action<float> onDamage;
    public event Action onDead;

    private WeaponBuffer buffer;
    private bool hasDebuff;
    protected bool overrideSpeed = false; //빙결 디버프용


    protected EntityData data;
    protected virtual EntityData EntityData => data;
    public float GetToolWear() => toolWear;
    
    public float Hp => hp;
    public float Defense => defense;

    protected virtual void Awake()
    {
        if (!ObjectPoolManager.Instance.IsPoolRegistered(data.EntityId))
            ObjectPoolManager.Instance.RegisterPrefab(data.EntityId, data.EntityPrefab);
    }

    protected virtual void OnEnable()
    {
        IsDead = false;
        hasDebuff = false;
        overrideSpeed = false;
        
        if(debuffIcon)
            debuffIcon.gameObject.SetActive(false);
        
        if(healthSlider)
        {
            healthSlider.maxValue = hp;
            healthSlider.value = hp;
            healthSlider.gameObject.SetActive(true);
        }
    }

    public virtual void StopMove()
    {
        if (agent != null)
        {
            agent.enabled = false;  // 비활성화하여 이동을 멈추게 함
            agent.speed = 0f;       // 속도 0으로 설정
        }
    }

    public virtual void MoveRandom() { }
    public bool IsStopped()
    {
        // agent.enabled가 false이면 에이전트는 멈춘 상태로 처리
        return !agent.enabled || !agent.hasPath;
    }
    

    public virtual void OnDamage(float damage, bool isCritical)
    {
        if (IsDead)
            return;
        
        damage *= (1 - defense);
        
        ApplyDamageEffect(damage, isCritical);
        onDamage?.Invoke(damage);

        Debug.Log("체력 줄일게");
        hp -= damage;
        healthSlider.value -= damage;
        
        
        if (hp <= 0)
        {
            Dead();
        }
    }

    public virtual void OnDebuffDamage(DamageType damageType, DebuffType debuffType, float damage)
    {
        if (IsDead)
            return;
        
        DamageNumber damageNumber = DamageManager.Instance.GetDamageSkin(damageType);
        damageNumber.Spawn(transform.position, damage);
        damageNumber.SetFollowedTarget(transform);

        ObjectPoolManager.Instance.SpawnObject((int)damageType, transform.position, Quaternion.identity);
        Debug.Log("디버프 체력 줄일게");
        hp -= damage;
        healthSlider.value -= damage;
        onDamage?.Invoke(damage);

        if (hp <= 0)
        {
            Dead();
        }
    }

    private void ApplyDamageEffect(float damage, bool isCritical)
    {
        DamageType type = isCritical ? DamageType.critical : DamageType.normal;
        DamageNumber damageNumber = DamageManager.Instance.GetDamageSkin(type);

        damageNumber.Spawn(transform.position, damage);
        damageNumber.SetFollowedTarget(transform);

        ObjectPoolManager.Instance.SpawnObject((int)type, transform.position, Quaternion.identity);
    }

    public void ApplyDebuff(WeaponBuffer buffer)
    {
        if (IsDead) return;
        if (hasDebuff) return;

        this.buffer = buffer.CreateBuffer(buffer.debuffLevel);
        debuffIcon.sprite = DamageManager.Instance.GetDebuffImage(buffer.currentDebuff.debuffType);
        debuffIcon.gameObject.SetActive(true);
        hasDebuff = true;
        this.buffer.ApplyEffect(this, this, this);
    }

    public void OnEndDebuff()
    {
        debuffIcon.gameObject.SetActive(false);
        hasDebuff = false;
        overrideSpeed = false;
        buffer = null;
    }

    private void Dead()
    {
        if (agent != null)
        {
            agent.enabled = false; // 몬스터가 죽었을 때 비활성화
        }

        healthSlider.gameObject.SetActive(false);
        buffer?.RemoveDebuff();
        IsDead = true;
        onDead?.Invoke();
    }

    public void DisableObject()
    {
        int entityId = EntityData.EntityId;
        DropItem(EntityData);

        ObjectPoolManager.Instance.ReleaseObject(entityId, gameObject);
    }

    private void DropItem(EntityData entityData)
    {
        GameEventsManager.Instance.playerEvents.GainExperience(entityData.DropTable.experience);

        foreach (var drop in entityData.DropTable.items)
        {
            int amount = drop.CalculateAndGetPrefab();
            for (int i = 0; i < amount; i++)
            {
                if (!ObjectPoolManager.Instance.IsPoolRegistered(drop.dropItem.ID))
                    ObjectPoolManager.Instance.RegisterPrefab(drop.dropItem.ID, drop.dropItem.DropObjectPrefab);

                Vector3 pos = transform.position + new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.2f, 0.2f), 0);
                GameObject dropObj = ObjectPoolManager.Instance.SpawnObject(drop.dropItem.ID, pos, Quaternion.identity);
                dropObj?.transform.DOJump(pos, 0.5f, 1, 0.8f).SetEase(Ease.OutBounce);
            }
        }
    }
    
    public void OverrideAgentSpeed()
    {
        overrideSpeed = true;
        agent.speed = 0f;
    }

    public void ReleaseAgentSpeed()
    {
        overrideSpeed = false;
        agent.speed = EntityData.MoveSpeed;
    }

    
    /// <summary>
    /// 몬스터를 죽이는게 아닌 그냥 사라지게 만들기
    /// </summary>
    public void ReleasePool()
    {
        ObjectPoolManager.Instance.ReleaseObject(data.EntityId, gameObject);
        StopAllCoroutines();
    }
    
    public virtual void SetTarget(Player player, Transform pos) {}
    
    public virtual void SetLevel(int level) {}
}
