using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackPoint : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private LineWeaponData emptyHand;
    private WeaponItem handWeapon;
    private readonly string rightButton = "rightButton";


    private Vector2 attackDir;
    private WaitForSeconds aniDelay = new WaitForSeconds(0.15f);
    private float lastAttackTime = 0f;

    private float lastSkillUsedTime;
    
    private void Start()
    {
        lastAttackTime = 0f;
        lastSkillUsedTime = -9999f;
        handWeapon = new LineWeaponItem(emptyHand);
    }

    public void TryAttack(InputAction.CallbackContext context, Item item)
    {
        if (context.control.name == rightButton)
        {
            Attack(item);
            return;
        }
        
        if (context.control == Keyboard.current.eKey)
        {
            UseSkill(item);
        }
    }

    private void Attack(Item item)
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        attackDir = (mouseWorldPos - (Vector2)transform.position).normalized;
        
        
        if (item is WeaponItem weaponItem)
        {
            if(Time.time >= lastAttackTime + weaponItem.AttackDelay)
            {
                _animator.SetTrigger("isAttack");
                StartCoroutine(AnimationDelayWeapon(weaponItem));
                lastAttackTime = Time.time;
            }
        }
        else
        {
            if(Time.time >= lastAttackTime + emptyHand.AttackDelay)
            {
                _animator.SetTrigger("isAttack");
                StartCoroutine(AnimationDelay());
                lastAttackTime = Time.time;
            }
        }
    }


    private void UseSkill(Item item)
    {
        if (item is WeaponItem weaponItem)
        {
            SkillData skillData = weaponItem.data.Skill;
            if (skillData != null)
            {
                if (Time.time - lastSkillUsedTime >= skillData.CoolDown)
                {
                    ActivateSkill(weaponItem);
                    lastSkillUsedTime = Time.time;
                }
            }
        }
    }

    private void ActivateSkill(WeaponItem weaponItem)
    {
        Debug.Log("액티베이트스킬");
        SkillData skillData = weaponItem.data.Skill;
        
        PlayerSkill playerSkill = Instantiate(
            skillData.SkillPrefab,
            transform.position,
            Quaternion.identity).GetComponent<PlayerSkill>();
        
        playerSkill.Init(skillData);
        playerSkill.ActiveSkill();
    }

    private IEnumerator AnimationDelayWeapon(WeaponItem weaponItem)
    {
        yield return aniDelay;
        weaponItem.ExecuteAttack(attackDir, transform.position);
    }
    
    private IEnumerator AnimationDelay()
    {
        yield return aniDelay;
        handWeapon.ExecuteAttack(attackDir, transform.position);
    }
}
