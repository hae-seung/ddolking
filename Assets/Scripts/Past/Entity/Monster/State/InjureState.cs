using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjureState : BaseState
{
    public override bool CanTransition => false;
    
    public InjureState(MonsterController controller) : base(controller) {}

    public override void EnterState()
    {
        controller.PlayInjureAnim();
    }

    public override void UpdateState()
    {
       
    }

    public override void ExitState()
    {
        
    }
}
