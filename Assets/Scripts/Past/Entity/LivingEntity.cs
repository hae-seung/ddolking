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

public abstract class LivingEntity : MonoBehaviour, IDamageable
{
    [SerializeField] protected Slider healthSlider;
    [SerializeField] private Image debuffIcon;

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
    protected bool overrideSpeed = false;

    protected virtual EntityData EntityData => null;

    protected virtual void Awake()
    {
        hp = EntityData.Hp;
        toolWear = EntityData.ToolWear;

        healthSlider.maxValue = hp;
        healthSlider.value = hp;
    }

    protected virtual void OnEnable()
    {
        healthSlider.maxValue = hp;
        healthSlider.value = hp;
        healthSlider.gameObject.SetActive(true);

        hasDebuff = false;
        overrideSpeed = false;
        debuffIcon.gameObject.SetActive(false);
    }

    public virtual void StopMove()
    {
        agent.isStopped = true;
        agent.speed = 0f;
    }

    public virtual void MoveRandom() { }

    public virtual void OnDamage(float damage, bool isCritical)
    {
        ApplyDamageEffect(damage, isCritical);
        onDamage?.Invoke(damage);

        damage *= (1 - defense);
        hp -= damage;
        healthSlider.value -= damage;

        if (hp <= 0)
        {
            OnDead();
        }
    }

    public void OnDebuffDamage(DamageType damageType, DebuffType debuffType, float damage)
    {
        DamageNumber damageNumber = DamageManager.Instance.GetDamageSkin(damageType);
        damageNumber.Spawn(transform.position, damage);
        damageNumber.SetFollowedTarget(transform);

        ObjectPoolManager.Instance.SpawnObject((int)damageType, transform.position, Quaternion.identity);

        hp -= damage;
        healthSlider.value -= damage;
        onDamage?.Invoke(damage);

        if (hp <= 0)
        {
            OnDead();
        }
    }

    public float GetToolWear() => toolWear;

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
        if (hasDebuff) return;

        this.buffer = buffer.CreateBuffer(buffer.debuffLevel);
        debuffIcon.sprite = DamageManager.Instance.GetDebuffImage(buffer.currentDebuff.debuffType);
        debuffIcon.gameObject.SetActive(true);
        hasDebuff = true;
        this.buffer.ApplyEffect(this);
    }

    public void OnEndDebuff()
    {
        Debug.Log("버프가 끝나서 아이콘을 끕니다");
        debuffIcon.gameObject.SetActive(false);
        hasDebuff = false;
        overrideSpeed = false;
        buffer = null;
    }

    private void OnDead()
    {
        healthSlider.gameObject.SetActive(false);
        buffer?.RemoveDebuff();
        onDead?.Invoke();
    }

    public void DisableObject()
    {
        int entityId = EntityData.EntityId;
        DropItem(EntityData);

        if (!ObjectPoolManager.Instance.IsPoolRegistered(entityId))
            ObjectPoolManager.Instance.RegisterPrefab(entityId, gameObject);

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
}
