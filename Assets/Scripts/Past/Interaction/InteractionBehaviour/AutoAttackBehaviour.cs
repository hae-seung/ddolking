using System.Collections;
using UnityEngine;

public class AutoAttackBehaviour : MonoBehaviour
{
    [SerializeField] private RangeWeaponData weaponItemData;

    private Animator animator;
    private WeaponItem weapon;
    
    private WaitForSeconds delay;
    private string isAttck = "isAttack";

    private void Awake()
    {
        weapon = weaponItemData.CreateItem() as WeaponItem;
        animator = GetComponent<Animator>();
        delay = new WaitForSeconds(weaponItemData.AttackDelay);
    }

    private void OnEnable()
    {
        StartCoroutine(AttackLoop());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            animator.SetBool(isAttck, true); // 애니메이션 시작
            yield return delay; // 전체 쿨타임 대기
        }
    }

    // 애니메이션 끝에 이 이벤트 연결
    public void PerformAttack()
    {
        Vector2 dir = Vector2.right;
        Vector2 origin = transform.position;
        weapon.ExecuteAttack(dir, origin);
    }

    public void EndAnimation()
    {
        animator.SetBool(isAttck, false); // 공격 끝 → 애니메이션 idle로
    }
}