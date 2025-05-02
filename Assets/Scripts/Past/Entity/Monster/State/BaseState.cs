

public abstract class BaseState
{
    protected MonsterController controller;
    
    public StateType? RequestedState { get; protected set; }

    public virtual bool CanTransition => true;
    
    protected BaseState(MonsterController controller)
    {
        this.controller = controller;
    }
    
    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

}
