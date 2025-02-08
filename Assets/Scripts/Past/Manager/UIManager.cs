using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject quickSlotUI;
    
    public GameObject invenStatusTab;
    public GameObject craftTab;
    public GameObject settingTab;
    public PopupUI popupUI;
    
    private bool isActiveInven = false;

    private void Awake()
    {
        invenStatusTab.SetActive(false);
        craftTab.SetActive(false);
        settingTab.SetActive(false);
        popupUI.FirstAwake();
        
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.inputEvents.onInventoryToggle += ToggleInventory;
        GameEventsManager.Instance.inputEvents.onEscPressed += EscPressed;
    }

    private void EscPressed()
    {
        // Setting → Craft → Inventory 순서로 닫기
        if (settingTab.activeSelf)
        {
            settingTab.SetActive(false);
        }
        else if (craftTab.activeSelf)
        {
            craftTab.SetActive(false);
        }
        else if (invenStatusTab.activeSelf)
        {
            invenStatusTab.SetActive(false);
            isActiveInven = false;
        }
        else
        {
            // 모든 UI가 닫혀 있으면 Setting을 열기
            settingTab.SetActive(true);
        }
    }

    private void ToggleInventory()
    {
        if (settingTab.activeSelf) 
            return; 
    
        isActiveInven = !isActiveInven;
        invenStatusTab.SetActive(isActiveInven);
        
        if (!isActiveInven)
        {
            craftTab.SetActive(false);
        }
    }
}