using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public QuickSlotUI quickSlotUI;
    public GameObject gripItemPrefab;
    private int curQuickSlotIndex;
    
    private GameObject currentHandItem;
    
    private void Awake()
    {
        curQuickSlotIndex = 0;
        quickSlotUI.OnChangedQuickSlot += HandleQuickSlotChanged; // 퀵슬롯에 아이템이 들어왔을 때
        quickSlotUI.OnQuickSlotEmptied += HandleQuickSlotEmptied; // 퀵슬롯에서 아이템이 나갔을 때
    }

    // 플레이어가 퀵슬롯 선택을 변경할 때
    public void HandQuickSlot(int index)
    {
        if (curQuickSlotIndex != index)
        {
            Item quickItem = quickSlotUI.MoveQuickSlot(curQuickSlotIndex, index);

            // 1. 손에 들고 있는 아이템 제거
            if (transform.childCount > 0)
            {
                Transform gripItemTrans = transform.GetChild(0);
                Destroy(gripItemTrans.gameObject);
            }

            // 2. 새로운 아이템을 손에 들기 (퀵슬롯 아이템)
            if (quickItem != null)
            {
                currentHandItem = Instantiate(gripItemPrefab, transform);
                GripItem gripItemScript = currentHandItem.GetComponent<GripItem>();
                gripItemScript.Init(quickItem);
            }

            curQuickSlotIndex = index;
        }
    }

    // 퀵슬롯 변경 처리: 다른 슬롯에서 퀵슬롯으로 아이템이 들어왔을 때
    private void HandleQuickSlotEmptied()
    {
        // 현재 손에 들고 있는 아이템이 있으면 제거
        if (currentHandItem != null)
        {
            Destroy(currentHandItem); // 손에 든 아이템 제거
            currentHandItem = null;
        }
    }

    private void HandleQuickSlotChanged(Item newItem)
    {
        // 현재 손에 든 아이템이 있으면 제거하고 새 아이템으로 교체
        if (currentHandItem != null)
        {
            Destroy(currentHandItem);
        }

        currentHandItem = Instantiate(gripItemPrefab, transform);
        GripItem gripItemScript = currentHandItem.GetComponent<GripItem>();
        gripItemScript.Init(newItem);
    }

   
}

