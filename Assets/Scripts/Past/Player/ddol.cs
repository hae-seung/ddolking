using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ddol : MonoBehaviour
{
    public InventoryUI inventoryUI;
    public PlayerHand hand;
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
            inventoryUI.OpenCloseInventory();
        
        if(Input.GetKeyDown(KeyCode.Alpha1))
            hand.HandQuickSlot(0);
        if(Input.GetKeyDown(KeyCode.Alpha2))
            hand.HandQuickSlot(1);
        if(Input.GetKeyDown(KeyCode.Alpha3))
            hand.HandQuickSlot(2);
        if(Input.GetKeyDown(KeyCode.Alpha4))
            hand.HandQuickSlot(3);
        if(Input.GetKeyDown(KeyCode.Alpha5))
            hand.HandQuickSlot(4);
    }
}