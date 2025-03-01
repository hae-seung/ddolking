using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;

public class CraftManualTables : MonoBehaviour
{
    [Header("테이블")]
    [SerializeField] private GameObject miniCraftTable;
    [SerializeField] private GameObject craftTable;
    [SerializeField] private GameObject furnaceTable;
    [SerializeField] private GameObject cookerTable;

    [SerializeField] private GameObject tableLog;
    
    public bool IsOpen { get; private set; }
    
    private void Awake()
    {
        Hide();
    }

    private void Hide()
    {
        miniCraftTable.SetActive(false);
        // craftTable.SetActive(false);
        // furnaceTable.SetActive(false);
        // cookerTable.SetActive(false);
        
        tableLog.SetActive(false);
    }

    public void ToggleTable(CraftManualType type)
    {
        if (IsOpen)
        {
            CloseTable();
            return;
        }
        
        switch (type)
        {
            case CraftManualType.MiniTable:
                miniCraftTable.SetActive(true);
                break;
        }
        GameEventsManager.Instance.playerEvents.DisablePlayerMovement();
        GameEventsManager.Instance.inputEvents.DisableInput();
        IsOpen = true;
    }

    public void CloseTable()
    {
        Hide();
        
        GameEventsManager.Instance.playerEvents.EnablePlayerMovement();
        GameEventsManager.Instance.inputEvents.EnableInput();
        
        IsOpen = false;//한개만 열리도록 보장
    }
}
