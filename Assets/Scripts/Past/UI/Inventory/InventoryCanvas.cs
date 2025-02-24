using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryCanvas : MonoBehaviour
{
    [SerializeField] private GameObject inventoryGo;
    [SerializeField] private PopupUI popupUI;

    private bool isOpen = false;

    public bool IsOpen => isOpen;
    
    private void Awake()
    {
        inventoryGo.SetActive(true);//하위 오브젝트 StatusUI의 Awake를 실행시킴
        inventoryGo.SetActive(false);
        
        
        popupUI.gameObject.SetActive(false);
        
        popupUI.Init();
        
        isOpen = false;
    }
    

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryGo.SetActive(isOpen);
    }
}
