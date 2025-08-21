using System.Collections;
using System.Collections.Generic;
using DamageNumbersPro;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour ,IDamageable, IDebuffable
{
    [SerializeField] private float shieldDuration = 90f;
    [SerializeField] private float minConvertTime = 10f;
    [SerializeField] private float maxConvertTime = 16f;

    
    [SerializeField] private Animator animator;
    [SerializeField] private string param;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image debuffIcon;
    
    
    private int id;
    private float hp;
    private BossAI boss;
    private float defense;
    private float toolWear;
    private float bossOrigin;

    private WeaponBuffer buffer;
    private bool hasDebuff;
    private Coroutine shieldRoutine;


    public void Init(int id, float hp, BossAI boss, float convertDefense, float toolWear)
    {
        if(shieldRoutine != null)
            StopCoroutine(shieldRoutine);
        
        
        this.id = id;
        this.hp = hp;
        this.boss = boss;
        defense = convertDefense;
        this.toolWear = toolWear;

        hasDebuff = false;

        healthSlider.maxValue = hp;
        healthSlider.value = hp;
        
        debuffIcon.gameObject.SetActive(false);
        healthSlider.gameObject.SetActive(true);

        bossOrigin = boss.Defense;
        
        //쉴드는 90초 지속에 쿨타임 120초
        //체력은 상대적으로 많이 낮을거임.
        //전화되는 시간은 랜덤 (10~16)
        SetPosition();
        shieldRoutine = StartCoroutine(StartShield());
    }

    private void SetPosition()
    {
        transform.SetParent(boss.transform, false);

        foreach (Transform child in boss.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name.Equals("Rig RPalm"))
            {
                transform.SetParent(child, false);
                transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
                return;
            }
        }
    }



    private IEnumerator StartShield()
    {
        boss.onDead += Dead;

        bool convert = false;
        float elapsed = 0f;
        float convertTime = Random.Range(minConvertTime, maxConvertTime);
        
        boss.SetDefense(0.99f);
        defense = 0.1f;
        animator.SetBool(param, false);
        
        //처음에는 방패가 눈을 뜬채로 시작 = 보스 단단해진채로 시작
        while (elapsed < shieldDuration)
        {
            yield return new WaitForSeconds(convertTime);
            elapsed += convertTime;

            if (!convert)
            {
                //방패 눈이 감김 => 보스 물렁해짐
                convert = true;
                boss.SetDefense(bossOrigin);
                defense = 0.99f;
                animator.SetBool(param, convert);
            }
            else
            {
                //방패 떠짐 => 보스 단단해짐
                convert = false;
                bossOrigin = boss.Defense;
                boss.SetDefense(0.99f);
                defense = 0.1f;
                animator.SetBool(param, convert);
            }
            
            convertTime = Random.Range(minConvertTime, maxConvertTime);
        }

        boss.SetDefense(bossOrigin);
        Dead();
    }


    private void Dead()
    {
        boss.onDead -= Dead;
        
        if(shieldRoutine != null)
            StopCoroutine(shieldRoutine);
        
        buffer?.RemoveDebuff(); 
        
        boss.SetDefense(bossOrigin);
        ObjectPoolManager.Instance.ReleaseObject(id, gameObject);
    }
    
    
    public void OnDamage(float damage, bool isCritical)
    {
        if (!gameObject.activeSelf)
            return;
        
        damage *= (1 - defense);
        
        ApplyDamageEffect(damage, isCritical);
        
        hp -= damage;
        healthSlider.value -= damage;
        
        
        if (hp <= 0)
        {
           Dead();
        }
    }
    
    public float GetToolWear()
    {
        return toolWear;
    }

    private void ApplyDamageEffect(float damage, bool isCritical)
    {
        if (!gameObject.activeSelf)
            return;
        
        DamageType type = isCritical ? DamageType.critical : DamageType.normal;
        DamageNumber damageNumber = DamageManager.Instance.GetDamageSkin(type);

        damageNumber.Spawn(transform.position, damage);
        damageNumber.SetFollowedTarget(transform);

        ObjectPoolManager.Instance.SpawnObject((int)type, transform.position, Quaternion.identity);
    }
    
    
    public void ApplyDebuff(WeaponBuffer buffer)
    {
        if (hasDebuff || buffer.currentDebuff is IceDebuffBase || !gameObject.activeSelf)
            return;

        this.buffer = buffer.CreateBuffer(buffer.debuffLevel);

        debuffIcon.sprite = DamageManager.Instance.GetDebuffImage(buffer.currentDebuff.debuffType);
        debuffIcon.gameObject.SetActive(true);
        
        hasDebuff = true;
        this.buffer.ApplyEffect(this, this, this);
    }

    
    public void OnEndDebuff()
    {
        if (!gameObject.activeSelf)
            return;
        
        debuffIcon.gameObject.SetActive(false);
        hasDebuff = false;
        buffer = null;
    }
    
    public void OnDebuffDamage(DamageType damageType, DebuffType debuffType, float damage)
    {
        if (!gameObject.activeSelf)
            return;
        
        Debug.Log(damage);
        
        DamageNumber damageNumber = DamageManager.Instance.GetDamageSkin(damageType);
        damageNumber.Spawn(transform.position, damage);
        damageNumber.SetFollowedTarget(transform);

        ObjectPoolManager.Instance.SpawnObject((int)damageType, transform.position, Quaternion.identity);

        hp -= damage;
        healthSlider.value -= damage;

        if (hp <= 0)
        {
            Dead();
        }
    }

}
