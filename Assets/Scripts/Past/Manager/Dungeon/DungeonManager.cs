using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;

    [SerializeField] private SpringDungeon springDungeon;
    [SerializeField] private SummerDungeon summerDungeon;
    [SerializeField] private AutumnDungeon autumnDungeon;
    [SerializeField] private WinterDungeon winterDungeon;

    [SerializeField] private float springLens;
    [SerializeField] private float summerLens;
    [SerializeField] private float autumnLens;
    [SerializeField] private float winterLens;
    
    
    private CinemachineVirtualCamera cam;
    
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

        cam = null;
    }

    public void EnterDungeon(DungeonType type, bool hasClear, Action ClearDungeon)
    {
        cam = VirtualCameraManager.Instance.GetCamera();
        cam.gameObject.SetActive(false);
        GameObject dc = VirtualCameraManager.Instance.GetCamera(CameraType.dungeon);
        dc.SetActive(true);

        var dungeonCamera = dc.GetComponent<CinemachineVirtualCamera>();
        
        
        switch (type)
        {
            case DungeonType.Spring:
                springDungeon.Enter(hasClear, ClearDungeon);
                dungeonCamera.m_Lens.OrthographicSize = springLens;
                break;
            case DungeonType.Summer:
                summerDungeon.Enter(hasClear, ClearDungeon);
                dungeonCamera.m_Lens.OrthographicSize = summerLens;
                break;
            case DungeonType.Autumn:
                autumnDungeon.Enter(hasClear, ClearDungeon);
                dungeonCamera.m_Lens.OrthographicSize = autumnLens;
                break;
        }
    }

    public void ExitDungeon(DungeonType type)
    {
        if(cam != null)
        {
            cam.gameObject.SetActive(true);
            VirtualCameraManager.Instance.GetCamera(CameraType.dungeon).SetActive(false);
        }
        
        switch (type)
        {
            case DungeonType.Spring:
                springDungeon.Exit();
                break;
            case DungeonType.Summer:
                summerDungeon.Exit();
                break;
            case DungeonType.Autumn:
                autumnDungeon.Exit();
                break;
        }
        
        UIManager.Instance.HideBossHealth();
        UIManager.Instance.HideTimer();
    }
    
}
