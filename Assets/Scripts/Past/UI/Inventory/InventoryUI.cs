using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private List<Slot> slots = new List<Slot>();

    public QuickSlotUI quickSlotUI;
    public RectTransform quickSlotParent;
    public RectTransform contentParent;
    public GameObject slotPrefab;

    
    public void AddNewItem(int idx, Item item)
    {
        if (0 <= idx && idx <= 4)
            quickSlotUI.CreateItem(idx, item);

        slots[idx].CreateItem(item);
    }

    public void UpdateItemAmount(int idx, int amount)
    {
        if (0 <= idx && idx <= 4)
            quickSlotUI.UpdateItemAmount(idx, amount);

        slots[idx].UpdateItemAmount(amount);
    }
    
    public void Init(int count)
    {
        int quickSlotCnt = 5;
        for (int i = 0; i < quickSlotCnt; i++)
        {
            Slot slot = Instantiate(slotPrefab, quickSlotParent).GetComponent<Slot>();
            slot.SetIndex(i);
            slots.Add(slot);
        }
        
        for (int i = quickSlotCnt; i < count; i++)
        {
            Slot slot = Instantiate(slotPrefab, contentParent).GetComponent<Slot>();
            slot.SetIndex(i);
            slots.Add(slot);
        }
    }

    public void AddSlot(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Slot slot = Instantiate(slotPrefab, contentParent).GetComponent<Slot>();
            slot.SetIndex(slots.Count);
            slots.Add(slot);
        }
    }

    
}
