using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : Singleton<UIManager> //모든 캔버스를 관통하는 통로 역할
{ 
    [SerializeField] private InventoryCanvas inventoryCanvas;
    [SerializeField] private GameObject SettingCanvas;
    [SerializeField] private CraftManualTables craftCanvas;
    [SerializeField] private GameObject buildPanel;

    private bool disableInput = false;
    
    
    private void OnEnable()
    {
        GameEventsManager.Instance.inputEvents.onInteractPressed += InteractPressed;
        GameEventsManager.Instance.inputEvents.onEscPressed += EscPressed;
        GameEventsManager.Instance.inputEvents.onEnableInput += EnableInput;
        GameEventsManager.Instance.inputEvents.onDisableInput += DisableInput;
    }
    

    private void InteractPressed(InputAction.CallbackContext context)
    {
        if (disableInput)
            return;
        
        if(context.control.name.Equals("tab"))
            ToggleInventory();
    }

    private void EscPressed()
    {
        //Setting → Craft → Inventory 순서로 닫기
        if (SettingCanvas.activeSelf)
        {
            SettingCanvas.SetActive(false);
        }
        else if (craftCanvas.IsOpen)
        {
            craftCanvas.CloseTable();
        }
        else if (inventoryCanvas.IsOpen)
        {
            inventoryCanvas.ToggleInventory();
        }
        else
        {
            SettingCanvas.SetActive(true);
        }
    }

    #region ToggleInventory

    private void ToggleInventory()
    {
        inventoryCanvas.ToggleInventory();
    }

    #endregion

    #region CraftTableToggle

    public void OpenCraftTab(CraftManualType type, Action<CraftItemSO , int> makeItem)
    {
        craftCanvas.OpenTable(type, makeItem);
    }

    public void CloseCraftTab()
    {
        craftCanvas.CloseTable();
    }

    #endregion
    
    #region BuildSystem

    public void ToggleBuildPanel(bool state)
    {
        if (state)
            HideOtherCanvas();

        buildPanel.SetActive(state);
    }

    private void HideOtherCanvas()
    {
        if(inventoryCanvas.IsOpen)
            inventoryCanvas.ToggleInventory();
        
        if(craftCanvas.IsOpen)
            craftCanvas.CloseTable();
        
        //setting 닫기
    }

    #endregion

    #region 입력 이벤트 제어

    private void DisableInput()
    {
        disableInput = true;
    }

    private void EnableInput()
    {
        disableInput = false;
    }

    #endregion
    
}