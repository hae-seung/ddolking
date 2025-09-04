using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousePortalInteraction : InteractionBehaviour
{
    [Header("포탈 활성화를 위한 아이템")]
    [SerializeField] private ItemData needPortalActiveItem;
    [SerializeField] private VillageType villageType;
    
    private bool isActivePortal;


    private void Awake()
    {
        isActivePortal = false;
    }


    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        if (!isActivePortal)
        {
            if (currentGripItem.itemData == needPortalActiveItem)
            {
                isActivePortal = true;
                UIManager.Instance.RegisterFieldShortCut(villageType);
                Inventory.Instance.RemoveItem(needPortalActiveItem, 1);
            }
        }
        
        OpenUI(interactor);
    }

    private void OpenUI(Interactor interactor)
    {
        UIManager.Instance.OpenFieldShortCut(interactor);
        GameEventsManager.Instance.inputEvents.DisableInput();
        GameEventsManager.Instance.playerEvents.DisablePlayerMovement();
    }
}
