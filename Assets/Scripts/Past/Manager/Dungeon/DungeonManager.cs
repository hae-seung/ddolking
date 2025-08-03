using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;

    [SerializeField] private SpringDungeon _springDungeon;
    [SerializeField] private SummerDungeon summerDungeon;
    [SerializeField] private AutumnDungeon _autumnDungeon;
    [SerializeField] private WinterDungeon winterDungeon;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnterDungeon(DungeonType type, bool hasClear)
    {
        switch (type)
        {
            case DungeonType.Spring:
                _springDungeon.Enter(hasClear);
                break;
        }
    }

    public void ExitDungeon(DungeonType type)
    {
        switch (type)
        {
            case DungeonType.Spring:
                _springDungeon.Exit();
                break;
        }
    }
    
}
