using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private Slot[] slots;
    public Transform slotHolder;
    private Inventory inven;
    public Sprite slotOpenImage;
    public GameObject inventoryFrame;
    public GameObject ItemPrefab; // 빈 아이템 프리팹
    public QuickSlotUI quickSlotUI;

    private bool activeInventory;
    
    public void Init(Inventory inventory)
    {
        inven = inventory;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        AddSlot(inven.SlotCnt); // 인벤토리 초기화
        
        for(int i = 0; i<slots.Length; i++)
            slots[i].SetIndex(i);
        
        activeInventory = false;
        inventoryFrame.SetActive(activeInventory);
        quickSlotUI.InitializeQuickSlots();
    }
    

    public void AddSlot(int val)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < val)
            {
                slots[i].GetComponent<Image>().sprite = slotOpenImage;
                slots[i].GetComponent<Image>().raycastTarget = true;
            }
            else
            {
                slots[i].GetComponent<Image>().raycastTarget = false;
            }
        }
    }

    public void AddNewItem(int idx, Item item)
    {
        GameObject newItem = Instantiate(ItemPrefab, slots[idx].transform);
        DragableItem dItem = newItem.GetComponent<DragableItem>();
        dItem.Init(item);
        dItem.ResizeItemImage();
        if (slots[idx].isQuickSlot)
            quickSlotUI.UpdateQuickSlot(idx);
    }

    public void UpdateItemAmount(int idx, Item item)
    {
        DragableItem dragableItem = slots[idx].GetComponentInChildren<DragableItem>();
        if (dragableItem != null && item is CountableItem ci)
        {
            dragableItem.itemAmount.text = ci.Amount.ToString();
        }
    }

    public void SwapItem(int index1, int index2)
    {
       inven.SwapItems(index1, index2);
    }

    public void OpenCloseInventory()
    {
        activeInventory = !activeInventory;
        inventoryFrame.SetActive(activeInventory);
    }
}
