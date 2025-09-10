using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    public override bool CanTransition => false; //다른 상태 전이 불가 -> 애니메이션 보장
    
    public AttackState(MonsterController controller) : base(controller) {}

    
    
    public override void EnterState()
    {
        controller.PlayAttackAnim();
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
