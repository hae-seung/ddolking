using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    private List<Item> _items = new List<Item>(); // 인벤토리 슬롯 리스트
    //0~4번 인덱스까지는 퀵슬롯을 위한 자리
    
    public InventoryUI inventoryUI;
    private int slotCnt;
    public int SlotCnt
    {
        get => slotCnt;
        set { slotCnt = value; }
    }

    protected override void Awake()
    {
        base.Awake();
        SlotCnt = 25; // 초기 슬롯 개수 설정
        inventoryUI.Init(SlotCnt);
        UpdateInventory(SlotCnt); // List는 0, 슬롯은 열려있어서 각각 맞춰줘야함
    }

    public void AddInventoryList(int count) // 배낭아이템이 사용
    {
        SlotCnt += count; // 슬롯 개수 증가
        inventoryUI.AddSlot(count); // 슬롯 해금
        UpdateInventory(count); // 인벤토리 List<> 해금
    }

    private void UpdateInventory(int count)
    {
        for (int i = 0; i < count; i++)
            _items.Add(null); // 새로 추가된 리스트는 null로 초기화
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
                // 1-1. 이미 해당 아이템이 인벤토리 내에 존재하고, 개수 여유 있는지 검사
                if (findNextCountable)
                {
                    index = FindCountableItemSlotIndex(countableItem, index + 1);
                    
                    // 개수 여유있는 기존재 슬롯이 더이상 없다고 판단될 경우, 빈 슬롯부터 탐색 시작
                    if (index == -1)
                    {
                        findNextCountable = false;
                    }
                    // 기존재 슬롯을 찾은 경우, 양 증가시키고 초과량 존재 시 amount에 초기화
                    else
                    {
                        CountableItem existingItem = _items[index] as CountableItem;
                        amount = existingItem.AddAmountAndGetExcess(amount); // 수량 추가 후 남은 수량 계산
                        inventoryUI.UpdateItemAmount(index, existingItem.Amount);
                    }
                }
                // 1-2. 빈 슬롯 탐색
                else
                {
                    index = FindEmptySlotIndex();
                    // 빈 슬롯조차 없는 경우 종료
                    if (index == -1)
                    {
                        Debug.LogWarning("빈 슬롯이 없습니다.");
                        return amount; // 남은 양 반환
                    }
                    // 빈 슬롯 발견 시, 슬롯에 아이템 추가 및 잉여량 계산
                    else
                    {
                        if (amount == 1)
                        {
                            _items[index] = item;
                            amount = 0;
                        }
                        else
                        {
                            CountableItem citem = countableItem.Clone(amount);
                        
                            _items[index] = citem;
                            amount -= citem.Amount;
                        }
                        inventoryUI.AddNewItem(index, _items[index]);
                    }
                }
            }
        }
        else //도구류
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
        (_items[index1], _items[index2]) = (_items[index2], _items[index1]);
    }
}
