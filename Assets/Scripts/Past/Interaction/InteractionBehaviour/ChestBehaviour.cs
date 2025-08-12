using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Chest : RebuildItem
{
    private int maxSlotCnt = 12;
    
    //상자 안에 담기는 실제 객체 리스트
    private List<Item> items;

    public int MaxSlotCnt => maxSlotCnt;


    public void Log()
    {
        for(int i = 0; i<12; i++)
            Debug.Log(items[i].itemData.Name);
    }
    
    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < maxSlotCnt;
    }
    
    public Chest(List<Item> datas)
    {
        items = new List<Item>(datas);

        if (items.Count < maxSlotCnt)
        {
            for (int i = items.Count; i < maxSlotCnt; i++)
                items.Add(null);
        }
    }

    public Chest()
    {
        items = new List<Item>();
        for(int i = 0; i<maxSlotCnt; i++)
            items.Add(null);
    }


    public int Add(Item item, int amount)
    {
        // 수량이 있는 아이템일 경우
        int index;
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
                        CountableItem existingItem = items[index] as CountableItem;
                        
                        amount = existingItem.AddAmountAndGetExcess(amount); // 수량 추가 후 남은 수량 계산
                        UIManager.Instance.UpdateChestSlot(index);
                    }
                }
                // 1-2. 빈 슬롯 탐색
                else
                {
                    index = FindEmptySlotIndex();
                    // 빈 슬롯조차 없는 경우 종료
                    if (index == -1)
                    {
                        return amount; // 아이템 획득 실패! 남은 양 반환 
                    }
                    // 빈 슬롯 발견 시, 슬롯에 아이템 추가 및 잉여량 계산
                    else
                    {
                        CountableItem citem = countableItem.Clone();
                        citem.SetAmount(amount);
                        
                        items[index] = citem;
                        amount = (amount > citem.MaxAmount) ? (amount - citem.MaxAmount) : 0;
                        
                        UIManager.Instance.UpdateChestSlot(index);
                    }
                }
            }
        }
        else //도구류
        {
            index = FindEmptySlotIndex();
            if (index != -1)
            {
                EquipItem eitem = item as EquipItem;
                items[index] = eitem.Clone();
                amount = 0;
                UIManager.Instance.UpdateChestSlot(index);
            }
        }
        
        return amount; // 남은 아이템 수량 반환
    }

    private int FindEmptySlotIndex()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    private int FindCountableItemSlotIndex(CountableItem countableItem, int startIndex = 0)
    {
        for (int i = startIndex; i < items.Count; i++)
        {
            if (items[i] is CountableItem existingItem)
            {
                if (existingItem.itemData.ID == countableItem.itemData.ID && !existingItem.IsMax)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    
    //인덱스로 해당 위치 아이템 amount만큼 제거
    public void RemoveItem(int index, int count = 1)
    {
        if (!IsValidIndex(index)) 
            return;

        if (items[index] is CountableItem citem)
        {
            int nextAmount = citem.Amount - count;
            if(nextAmount <= 0)
                items[index] = null;
            else
                citem.SetAmount(nextAmount);
        }
        else
            items[index] = null;
    }
    
    
    //인덱스로 해당 위치 아이템 가져오기
    public Item GetItem(int idx)
    {
        if (IsValidIndex(idx) && items[idx] != null)
            return items[idx];

        return null;
    }
    
}



public class ChestBehaviour : InteractionBehaviour, IReBuild
{
    [SerializeField] private List<ItemData> rewardList;
    [SerializeField] private EstablishItemData establishData;
    private List<Item> items = new List<Item>();
    
    private EstablishItem eitem;
    private InterBreakableObject interbreakable;
    
    private void Awake()
    {
        interbreakable = GetComponent<InterBreakableObject>();
        
        for (int i = 0; i < rewardList.Count; i++)
        {
            Item item = rewardList[i].CreateItem();
            items.Add(item);
        }

        Chest chest = new Chest(items);
        
        if (!interbreakable)
            return;

        eitem = new EstablishItem(establishData);
        eitem.SetRebuildItem(chest);
        interbreakable.SetEstablishItem(eitem);
    }

    
    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        GameEventsManager.Instance.inputEvents.DisableInput();
        GameEventsManager.Instance.playerEvents.DisablePlayerMovement();
        UIManager.Instance.OpenChest(eitem.GetRebuildItem() as Chest);
    }

    public void SetRebuildItem(EstablishItem item)
    {
        if (item.GetRebuildItem() == null)
        {
            items.Clear();
            for (int i = 0; i < rewardList.Count; i++)
            {
                Item newItem = rewardList[i].CreateItem();
                items.Add(newItem);
            }

            Chest newChest = new Chest(items);
            item.SetRebuildItem(newChest);
        }
        
        eitem = item;
        interbreakable.SetEstablishItem(eitem);
    }
}