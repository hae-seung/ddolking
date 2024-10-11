using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryPanel;
    public bool activeInventory = false;

    private void Start()
    {
        activeInventory = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            activeInventory = !activeInventory;
            inventoryPanel.SetActive(activeInventory);
        }
    }
}
