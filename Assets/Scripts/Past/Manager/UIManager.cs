using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIManager : Singleton<UIManager> //모든 캔버스를 관통하는 통로 역할
{ 
    [SerializeField] private InventoryCanvas inventoryCanvas;
    [SerializeField] private GameObject SettingCanvas;
    [SerializeField] private CraftManualTables craftCanvas;
    [SerializeField] private GameObject buildPanel;
    [SerializeField] private SignUI sign;
    [SerializeField] private ShopCanvas shopCanvas;
    [SerializeField] private TransitionUI transitionUI;
    [SerializeField] private MineUI mineUI;

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


    #region Transition

    public void StartTransition()
    {
        transitionUI.EnableTransitionUI();
    }

    #endregion

    #region ToggleInventory

    private void ToggleInventory()
    {
        inventoryCanvas.ToggleInventory();
    }

    #endregion

    #region ToggleCraftTable

    public void OpenCraftTab(CraftManualType type,int id, Action<CraftItemSO , int> makeItem)
    {
        if (craftCanvas.IsOpen)
            return;
        craftCanvas.OpenTable(type, id, makeItem);
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

    public void HideOtherCanvas()
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
    
    #region 표지판 탭

    public void OpenSignTab(int needLevel, int needMoney, Action UnlockSign)
    {
        sign.OpenSignTab(needLevel, needMoney, UnlockSign);
    }

    public void CloseSignTab()
    {
        sign.CloseSignTab();
    }

    #endregion

    #region ToggleShop

    public void OpenShop(ShopType shopType)
    {
        shopCanvas.OpenShop(shopType);
    }

    public void CloseShop()
    {
        shopCanvas.CloseShop();
    }
    
    public void ShopWarn(int amount)
    {
        shopCanvas.Warn(amount);
    }
    #endregion

    #region MineUI

    public void OpenMineUI(string name, string list,int remainTime ,Action EnterMine, Action ExitMine)
    {
        mineUI.OpenMineUI(name, list, remainTime,EnterMine, ExitMine);
    }

    #endregion


    
}