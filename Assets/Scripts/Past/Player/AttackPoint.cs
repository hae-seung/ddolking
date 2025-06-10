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

    private void Start()
    {
        lastAttackTime = 0f;
        handWeapon = new LineWeaponItem(emptyHand);
    }

    public void TryAttack(InputAction.CallbackContext context, Item item)
    {
        if (context.control.name != rightButton)
            return;
        
        

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
