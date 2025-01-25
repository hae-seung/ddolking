using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ddol : MonoBehaviour
{
    private UIManager uiManager;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            uiManager.HandleTab();

        if (Input.GetKeyDown(KeyCode.Escape))
            uiManager.HandleEscape();

    }
    
}