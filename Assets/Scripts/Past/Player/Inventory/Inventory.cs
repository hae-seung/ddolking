using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    private int slotCnt;
    public List<Item> _items = new List<Item>(); // 인벤토리 슬롯 리스트
    public InventoryUI inventoryUI;
    
    public int SlotCnt
    {
        get => slotCnt;
        set {  slotCnt = value; }
    }

    private void Start()
    {
        SlotCnt = 18; // 초기 슬롯 개수 설정
        inventoryUI.AddSlot(SlotCnt);
        UpdateInventory(SlotCnt); // List는 0, 슬롯은 열려있어서 각각 맞춰줘야함
    }
    
    public void AddInventoryList(int count)//배낭아이템이 사용
    {
        SlotCnt += count; //슬롯개수 증가
        inventoryUI.AddSlot(SlotCnt);//슬롯 해금
        UpdateInventory(count);//인벤토리 List<> 해금
    }
    
    private void UpdateInventory(int count)
    {
        for (int i = 0; i < count; i++)//반복횟수
        {
            _items.Add(null); // 새로 추가된 리스트는 null로 초기화
        }
        Debug.Log($"인벤토리 초기화됨: {_items.Count}개의 슬롯 생성.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            inventoryUI.OpenCloseInventory();
    }

   public int Add(Item item, int amount = 1)
    {   
        int index;

        // 수량이 있는 아이템일 경우
        if (item is CountableItem countableItem)
        {
            bool findNextCountable = true;
            index = -1;

            while (amount > 0) // 남은 수량이 0이 될 때까지 계속 반복
            {
                if (findNextCountable) // 같은 아이템이 있는지 찾기
                {
                    index = FindCountableItemSlotIndex(countableItem, index + 1);

                    if (index == -1) // 같은 아이템이 없거나, 수량이 가득 찬 경우
                    {
                        findNextCountable = false;
                    }
                    else // 같은 아이템의 수량을 추가
                    {
                        CountableItem existingItem = _items[index] as CountableItem;
                        amount = existingItem.AddAmountAndGetExcess(amount); // 수량 추가 후 남은 수량 계산

                        // 해당 슬롯의 수량 텍스트만 업데이트
                        inventoryUI.UpdateItemAmount(index, existingItem);
                    }
                }
                else // 빈 슬롯을 찾아서 나머지 수량을 추가
                {
                    index = FindEmptySlotIndex();
                
                    if (index == -1)
                    {
                        Debug.LogWarning("빈 슬롯이 없습니다.");
                        return amount; // 남은 양 반환 (빈 슬롯이 없으면 남은 양을 반환)
                    }
                    else
                    {
                        // 새로운 아이템 추가
                        int addableAmount = Mathf.Min(amount, countableItem.MaxAmount); // 남은 공간만큼 추가
                        CountableItem newItem = new CountableItem(countableItem.CountableData, addableAmount);
                        _items[index] = newItem;

                        // 남은 수량 계산
                        amount -= addableAmount;

                        // 새로운 슬롯에 아이템 추가
                        inventoryUI.AddNewItem(index, newItem);
                        Debug.Log($"슬롯 {index}에 아이템 추가됨: {newItem.itemData.Name}, 수량: {newItem.Amount}");
                    }
                }
            }
        }
        else // 수량이 없는 아이템일 경우
        {
            index = FindEmptySlotIndex();
    
            if (index != -1)
            {
                _items[index] = item;
                inventoryUI.AddNewItem(index, item);
            }
        }

        return amount; // 남은 아이템 수량 반환 (추가할 수 없었던 양)
    }

   

    

    // 빈 슬롯을 찾는 메서드
    private int FindEmptySlotIndex(int startIndex = 0)
    {
        for (int i = startIndex; i < _items.Count; i++)
        {
            if (_items[i] == null) // 빈 슬롯을 찾으면 해당 인덱스 반환
            {
                return i;
            }
        }
        return -1; // 빈 슬롯이 없으면 -1 반환
    }
    

    // 수량이 여유 있는 CountableItem의 슬롯을 찾는 메서드
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
        return -1; // 여유 있는 CountableItem이 없으면 -1 반환
    }
}




