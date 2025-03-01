

public class GameEventsManager : Singleton<GameEventsManager>
{
    //구독은 Awake나 onEnable에서
    //이벤트 실행 매소드는 최소 Start에서
    
    public InputEvents inputEvents;
    public PlayerEvents playerEvents;
    public StatusEvents statusEvents;
    
    protected override void Awake()
    {
        base.Awake();
        inputEvents = new InputEvents();
        playerEvents = new PlayerEvents();
        statusEvents = new StatusEvents();
    }
}
