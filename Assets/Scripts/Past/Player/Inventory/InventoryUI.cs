using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Slot[] slots;
    public Transform slotHolder;
    private Inventory inven;
    public Sprite slotOpenImage;
    public GameObject inventoryFrame;
    public GameObject ItemPrefab;//빈 아이템 프리팹
    public bool activeInventory;
    public QuickSlotUI quickSlotUI;
    
    private void Awake()
    {
        inven = Inventory.Instance;
        if (slots.Length == 0)//인스펙터에서 할당 안한 경우 대비
            slots = slotHolder.GetComponentsInChildren<Slot>();
        AddSlot(inven.SlotCnt);//인벤토리를 인스펙터에서 false로 시작할경우 대비
    }

    private void Start()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].SetIndex(i);
        activeInventory = false;
        inventoryFrame.SetActive(activeInventory);
        quickSlotUI.InitializeQuickSlots();
    }

    public void AddSlot(int val)
    {
        for (int i = 0; i < slots.Length; i++)//총 슬롯은 고정시켜둘거임
        {
            if (i < val)
            {
                slots[i].GetComponent<Image>().sprite = slotOpenImage;
                slots[i].GetComponent<Image>().raycastTarget = true; //잠긴 이미지는 활동하면 안됨
            }
            else
            {
                slots[i].GetComponent<Image>().raycastTarget = false;
            }
        }
    }

    
    public void SwapIndex(int index1, int index2)
    {
        int temp = index1;
        slots[index1].SetIndex(index2);
        slots[index2].SetIndex(temp);
    }
    
    public void OpenCloseInventory()
    {
        activeInventory = !activeInventory;
        inventoryFrame.SetActive(activeInventory);
    }

    public void AddNewItem(int idx, Item item)//인벤토리에서 일어난 변화를 슬롯에 적용
    {
        GameObject newItem = Instantiate(ItemPrefab, slots[slots[idx].SlotIndex].transform);
        DragableItem dItem = newItem.GetComponent<DragableItem>();
        dItem.Init(item);
        dItem.ResizeItemImage();
    }

    public void UpdateItemAmount(int idx, Item item)
    {
        // SlotIndex를 통해 실제 슬롯을 참조
        DragableItem dragableItem = slots[slots[idx].SlotIndex].GetComponentInChildren<DragableItem>();

        if (dragableItem != null && item is CountableItem ci)
        {
            dragableItem.itemAmount.text = ci.Amount.ToString(); // 수량 텍스트 업데이트
        }
    }
    
}