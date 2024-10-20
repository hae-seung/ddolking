using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ddol : MonoBehaviour
{
    public InventoryUI inventoryUI;
    

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
            inventoryUI.OpenCloseInventory();
    }
}