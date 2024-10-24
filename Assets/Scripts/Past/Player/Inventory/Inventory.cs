using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    public List<Item> _items = new List<Item>(); // 인벤토리 슬롯 리스트
    public InventoryUI inventoryUI;
    private int slotCnt;

    public int SlotCnt
    {
        get => slotCnt;
        set { slotCnt = value; }
    }

    private void Start()
    {
        SlotCnt = 18; // 초기 슬롯 개수 설정
        inventoryUI.AddSlot(SlotCnt);
        UpdateInventory(SlotCnt); // List는 0, 슬롯은 열려있어서 각각 맞춰줘야함
    }

    public void AddInventoryList(int count) // 배낭아이템이 사용
    {
        SlotCnt += count; // 슬롯 개수 증가
        inventoryUI.AddSlot(SlotCnt); // 슬롯 해금
        UpdateInventory(count); // 인벤토리 List<> 해금
    }

    private void UpdateInventory(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _items.Add(null); // 새로 추가된 리스트는 null로 초기화
        }
        Debug.Log($"인벤토리 초기화됨: {_items.Count}개의 슬롯 생성.");
    }

    public int Add(Item item, int amount = 1)
    {
        int index;

        // 수량이 있는 아이템일 경우
        if (item is CountableItem countableItem)
        {
            bool findNextCountable = true;
            index = -1;

            while (amount > 0)
            {
                if (findNextCountable)
                {
                    index = FindCountableItemSlotIndex(countableItem, index + 1);

                    if (index == -1)
                    {
                        findNextCountable = false;
                    }
                    else
                    {
                        CountableItem existingItem = _items[index] as CountableItem;
                        amount = existingItem.AddAmountAndGetExcess(amount); // 수량 추가 후 남은 수량 계산
                        inventoryUI.UpdateItemAmount(index, existingItem);
                    }
                }
                else
                {
                    index = FindEmptySlotIndex();
                    if (index == -1)
                    {
                        Debug.LogWarning("빈 슬롯이 없습니다.");
                        return amount; // 남은 양 반환
                    }
                    else
                    {
                        int addableAmount = Mathf.Min(amount, countableItem.MaxAmount); // 남은 공간만큼 추가
                        CountableItem newItem = new CountableItem(countableItem.CountableData, addableAmount);
                        _items[index] = newItem;
                        amount -= addableAmount;
                        inventoryUI.AddNewItem(index, newItem);
                    }
                }
            }
        }
        else
        {
            index = FindEmptySlotIndex();
            if (index != -1)
            {
                _items[index] = item;
                inventoryUI.AddNewItem(index, item);
            }
        }
        return amount; // 남은 아이템 수량 반환
    }

    private int FindEmptySlotIndex()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    private int FindCountableItemSlotIndex(CountableItem countableItem, int startIndex = 0)
    {
        for (int i = startIndex; i < _items.Count; i++)
        {
            if (_items[i] is CountableItem existingItem)
            {
                if (existingItem.itemData.ID == countableItem.itemData.ID && !existingItem.IsMax)
                {
                    return i;
                }
            }
        }
        return -1;
    }
    
    
    
    
    public void SwapItems(int index1, int index2)
    {
        // 아이템 리스트에서의 스왑
        Item temp = _items[index1];
        _items[index1] = _items[index2];
        _items[index2] = temp;

        Debug.Log($"Inventory items swapped: {index1} <-> {index2}");
    }
}
