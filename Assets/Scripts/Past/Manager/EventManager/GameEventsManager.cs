using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : Singleton<GameEventsManager>
{
    public InputEvents inputEvents;
    
    protected override void Awake()
    {
        base.Awake();
        inputEvents = new InputEvents();
    }
}
