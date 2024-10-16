using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    public void AddSlot(int val)
    {
        Debug.Log(slots.Length);
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

    public void OpenCloseInventory()
    {
        activeInventory = !activeInventory;
        inventoryFrame.SetActive(activeInventory);
    }

    public void AddNewItem(int idx, Item item)//인벤토리에서 일어난 변화를 슬롯에 적용
    {
       
    }
    
    
    [ContextMenu("show")]
    public void SHow()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Transform p = slots[i].transform;
            if(p.childCount >0 )
                Debug.Log(slots[i].SlotIndex +"번째 인덱스");
        }
    }
    
    
}