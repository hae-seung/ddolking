using System;
using System.Collections;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Monster monster;
    
    
    [Header("캐싱")]
    private string movingBlend = "MovingBlend";
    private string attack = "Attack";
    private WaitForSeconds dieTime = new WaitForSeconds(1f);
    

    
    //moving animation blend
    //0.0:idle,  0.5:walk,  1.0:run
    public void StopMove()
    {
        animator.SetFloat(movingBlend, 0f);
        monster.StopMove();
    }

    public void MoveRandom()
    {
        if (!monster.IsStopped())
        {
            return;
        }
        
        //움직이는 지속시간이 끝나기도 전에 목적지에 도착하거나 랜덤목적지를 잡지 못한경우에는 재실행
        animator.SetFloat(movingBlend, 0.5f);
        monster.MoveRandom();
    }

    public void ChasePlayer(bool isRunning)
    {
        animator.SetFloat(movingBlend, isRunning ? 1.0f : 0.5f);
        monster.ChasePlayer(isRunning);
    }
    
    public void PlayAttackAnim()
    {
        animator.SetTrigger(attack);
    }
    

    public void PlayInjureAnim()
    {
        //1. 오른쪽을 보는데 뒤치기 => injure back
        //2. 오른쪽을 보는데 앞치기 => injure front
        
        //3. 왼쪽을 보는데 앞치기 => injure front
        //4. 왼쪽을 보는데 뒤치기 => injure back

        if (monster.FacingRight)
        {
            if (monster.target.transform.position.x - transform.position.x >= 0)
            {
                animator.SetTrigger("InjuredFront");
            }
            else
            {
                animator.SetTrigger("InjuredBack");
            }
        }
        else
        {
            if (monster.target.transform.position.x - transform.position.x >= 0)
            {
                animator.SetTrigger("InjuredBack");
            }
            else
            {
                animator.SetTrigger("InjuredFront");
            }
        }
        
        monster.StopMove();
    }


    public void PlayDeadAnim()
    {
        animator.SetBool("IsDead", true);
        StartCoroutine(DisableMonster());
    }

    private IEnumerator DisableMonster()
    {
        yield return dieTime;
        monster.DisableObject();
    }
    
    
    public AnimatorStateInfo GetAnimatorState(int layerIndex)
    {
        return animator.GetCurrentAnimatorStateInfo(layerIndex);
    }

    
    public void Init(Monster monster)
    {
        this.monster = monster;
    }

    public void SetFacing(Vector3 facingDir)
    {
        animator.transform.localScale = facingDir;
    }
}
