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
    public bool activeInventory;
    
    private void Awake()
    {
        inven = Inventory.Instance;
        inven.onSlotCountChanged += SlotChange;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        activeInventory = false;
        gameObject.SetActive(false);

        // 인벤토리 UI가 초기화된 후 현재 SlotCnt 값에 따라 슬롯 상태 업데이트
        SlotChange(inven.SlotCnt);  // 현재 SlotCnt 값으로 슬롯을 갱신
    }

    private void SlotChange(int val)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inven.SlotCnt)
            {
                slots[i].GetComponent<Button>().interactable = true;
                slots[i].GetComponent<Image>().sprite = slotOpenImage;
            }
            else
            {
                slots[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    public void OpenCloseInventory()
    {
        activeInventory = !activeInventory;
        gameObject.SetActive(activeInventory);
    }

    public void AddSlot(int num)
    {
        inven.SlotCnt += num;
    }
}