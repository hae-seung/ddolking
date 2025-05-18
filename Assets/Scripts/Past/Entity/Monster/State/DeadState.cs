using UnityEngine;

public class DeadState : BaseState
{
    private bool hasRequestedTransition = false;

    public override bool CanTransition => false; // 죽는 동안 다른 상태 전이 금지

    public DeadState(MonsterController controller) : base(controller) {}

    public override void EnterState()
    {
        hasRequestedTransition = false;
        controller.PlayDeadAnim(); 
    }

    public override void UpdateState()
    {
       
    }

    public override void ExitState()
    {
        hasRequestedTransition = false;
        RequestedState = null;
    }
}