using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private bool hasAttacked = false;
    private float attackTriggerTime = 0.99f; // 애니메이션 중 70%쯤에 공격 판정

    public override bool CanTransition => false; //다른 상태 전이 불가 -> 애니메이션 보장
    
    public AttackState(MonsterController controller) : base(controller) {}

    
    
    public override void EnterState()
    {
        controller.PlayAttackAnim();
        Debug.Log("공격애니 시작");
        hasAttacked = false;
    }

    public override void UpdateState()
    {
        AnimatorStateInfo stateInfo = controller.GetAnimatorState(controller.AttackLayerIndex);

        if (!hasAttacked && stateInfo.normalizedTime >= attackTriggerTime)
        {
            Debug.Log("실제공격작동");
            controller.PerformAttack();
            hasAttacked = true;
        }

        if (stateInfo.normalizedTime >= 1f)
        {
            RequestedState = StateType.Idle;
        }
    }

    public override void ExitState()
    {
        hasAttacked = false;
        RequestedState = null;
    }
}
