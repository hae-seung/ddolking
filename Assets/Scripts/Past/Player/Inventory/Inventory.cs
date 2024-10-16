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

        // 1. 수량이 있는 아이템 (CountableItem)
        if (item is CountableItem countableItem)
        {
            bool findNextCountable = true;
            index = -1;

            while (amount > 0)//수량이 0 이상이면 다른 슬롯에 넣어서라도 계속 돌아야함
            {
                if (findNextCountable)//같은물건 찾기 시작
                {
                    index = FindCountableItemSlotIndex(countableItem, index + 1);

                    if (index == -1)//같은 아이템이 맥스 수량을 찍은 경우 or 같은 아이템이 없는 경우
                    {
                        findNextCountable = false;
                    }
                    else//같은 아이템에 갯수만 추가하기
                    {
                        CountableItem existingItem = _items[index] as CountableItem;//인벤에서 아이템을 가져와서
                        amount = existingItem.AddAmountAndGetExcess(amount);//수량 추가

                        UpdateSlot(index,amount);//todo: 수량만 추가하는 함수 필요
                    }
                }
                else//빈슬롯을 찾아서 아이템 추가
                {
                    index = FindEmptySlotIndex(index + 1);

                    if (index == -1)
                    {
                        Debug.LogWarning("빈 슬롯이 없습니다.");
                        return amount; // 남은 양 반환 (빈 슬롯이 없으므로 인벤토리에 모두 추가하지 못한 양)
                    }
                    else//빈슬롯을 찾은경우
                    {
                        //amount는 맥스치를 찍게 하고 남은 수량이거나 1개이거나였던 경우여서 빈슬롯을 찾음
                        CountableItem newItem = new CountableItem(countableItem.CountableData, amount);
                        _items[index] = newItem;//null대신 새로운 아이템추가

                        amount = (amount > countableItem.MaxAmount) ? (amount - countableItem.MaxAmount) : 0;

                        UpdateSlot(index,amount);
                        Debug.Log($"슬롯 {index}에 아이템 추가됨: {newItem.itemData.Name}, 수량: {newItem.Amount}");
                    }
                }
            }
        }
        else
        {
            // 수량이 없는 아이템 처리
            index = FindEmptySlotIndex();

            if (index == -1)
            {
                Debug.LogWarning("빈 슬롯이 없습니다.");
                return amount; // 빈 슬롯이 없으면 남은 양 반환
            }

            // 빈 슬롯에 아이템 추가
            _items[index] = item;
            UpdateSlot(index,amount);
            Debug.Log($"슬롯 {index}에 아이템 추가됨: {item.itemData.Name}");
        }

        return 0; // 성공적으로 모두 추가되었을 때 0 반환
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

    // 해금되어있던 빈슬롯에 아이템이 추가
    private void UpdateSlot(int index, int amount)
    {
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




