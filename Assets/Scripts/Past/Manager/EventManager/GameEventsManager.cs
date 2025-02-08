using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : Singleton<GameEventsManager>
{
    public InputEvents inputEvents;
    public PlayerEvents playerEvents;
    
    protected override void Awake()
    {
        base.Awake();
        inputEvents = new InputEvents();
        playerEvents = new PlayerEvents();
    }
}
