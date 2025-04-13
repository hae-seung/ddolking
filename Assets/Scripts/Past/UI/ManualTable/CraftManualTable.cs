using System;
using Unity.VisualScripting;
using UnityEngine;

public class CraftManualTables : MonoBehaviour
{
    [Header("테이블")]
    [SerializeField] private CraftTable craftTable;
    [SerializeField] private CraftTableLog tableLog;
    
    public bool IsOpen { get; private set; }
    
    private void Awake()
    {
        craftTable.Init();
        Hide();
    }

    private void Hide()
    {
        craftTable.gameObject.SetActive(false);
        tableLog.gameObject.SetActive(false);
    }

    public void OpenTable(CraftManualType type, Action<CraftItemSO , int> makeItem)
    {
        if (IsOpen)
            return;
        
        tableLog.SetConfirmEvent(makeItem);

        craftTable.OpenTable(type);
        
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
