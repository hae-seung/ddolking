using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjureState : BaseState
{
    private bool hasRequestedTransition = false;
    
    public override bool CanTransition => false;
    
    public InjureState(MonsterController controller) : base(controller) {}

    public override void EnterState()
    {
        hasRequestedTransition = false;
        controller.PlayInjureAnim();
    }

    public override void UpdateState()
    {
        AnimatorStateInfo stateInfo = controller.GetAnimatorState(controller.InjureLayerIndex);

        if (!hasRequestedTransition && stateInfo.normalizedTime >= 1f)
        {
            RequestedState = StateType.Idle;
            hasRequestedTransition = true;
        }
    }

    public override void ExitState()
    {
        hasRequestedTransition = false;
        RequestedState = null;
    }
}
