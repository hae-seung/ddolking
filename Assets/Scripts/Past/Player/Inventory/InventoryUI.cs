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
        if (slots.Length == 0)//인스펙터에서 할당 안한 경우 대비
            slots = slotHolder.GetComponentsInChildren<Slot>();
            
        activeInventory = false;
        gameObject.SetActive(false);
        AddSlot(inven.SlotCnt);//인벤토리를 인스펙터에서 false로 시작할경우 대비
    }

    public void AddSlot(int val)
    {
        Debug.Log(slots.Length);
        for (int i = 0; i < slots.Length; i++)//총 슬롯은 고정시켜둘거임
        {
            if (i < val)
            {
                slots[i].GetComponent<Image>().sprite = slotOpenImage;
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
        gameObject.SetActive(activeInventory);
    }
    
}