using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject quickSlotUI;
    public GameObject invenStatus;
    public GameObject craftTab;

    private bool isActiveInven = false;
    private bool isActiveCraft = false;
    
    private void Awake()
    {
        invenStatus.SetActive(false);
        craftTab.SetActive(false);
    }

    public void HandleTab()
    {
        if(isActiveInven)
            craftTab.SetActive(false);
        isActiveInven = !isActiveInven;
        invenStatus.SetActive(isActiveInven);
    }

    public void HandleEscape()
    {
        if (isActiveCraft)
        {
            isActiveCraft = !isActiveCraft;
            craftTab.SetActive(isActiveCraft);
            return;
        }

        if (isActiveInven)
        {
            isActiveInven = !isActiveInven;
            invenStatus.SetActive(isActiveInven);
            return;
        }
    }
    
}
