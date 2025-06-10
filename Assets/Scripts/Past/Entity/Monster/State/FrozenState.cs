using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenState : BaseState
{
    public override bool CanTransition => false;
    
    public FrozenState(MonsterController controller) : base(controller) { }

    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
