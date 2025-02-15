using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    private List<Item> _items = new List<Item>(); // 인벤토리 슬롯 리스트
    //0~4번 인덱스까지는 퀵슬롯을 위한 자리
    
    [SerializeField] private InventoryUI inventoryUI;
    private int slotCnt;
    public int SlotCnt
    {
        get => slotCnt;
        set { slotCnt = value; }
    }

    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < SlotCnt;
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);
        
        SlotCnt = 30; // 초기 슬롯 개수 설정
        inventoryUI.Init(SlotCnt, this);
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
        int initAmount = amount;
        
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
                        UpdateSlot(index);
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
                        return amount; // 아이템 획득 실패! 남은 양 반환 
                    }
                    // 빈 슬롯 발견 시, 슬롯에 아이템 추가 및 잉여량 계산
                    else
                    {
                        CountableItem citem = countableItem.Clone();
                        citem.SetAmount(amount);
                        
                        _items[index] = citem;
                        amount = (amount > citem.MaxAmount) ? (amount - citem.MaxAmount) : 0;
                        
                        UpdateSlot(index);
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
                amount = 0;
                UpdateSlot(index);
            }
        }
        
        GameEventsManager.Instance.playerEvents.AcquireItem(initAmount - amount, item.itemData.ID);
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

    
    public void RemoveItem(int index, int count)
    {
        if (!IsValidIndex(index)) 
            return;

        if (_items[index] is CountableItem citem)
        {
            int nextAmount = citem.Amount - count;
            if(nextAmount <= 0)
                _items[index] = null;
            else
                citem.SetAmount(nextAmount);
        }
        else
            _items[index] = null;
        
        UpdateSlot(index);
    }

    public void UseItem(int index)
    {
        if (!IsValidIndex(index))
            return;
        if (_items[index] == null)
            return;

        if (_items[index] is IUseable uItem)
        {
            bool succeeded = uItem.Use();
            if (succeeded)
            {
                UpdateSlot(index);
            }
        }

    }
    
    public void SwapItem(int from , int to)
    {
        (_items[from], _items[to]) = (_items[to], _items[from]);
    
        Item itemA = _items[from];
        Item itemB = _items[to];
    
        if (itemA != null && itemB != null &&
            itemA.itemData == itemB.itemData &&
            itemA is CountableItem ciA && itemB is CountableItem ciB)
        {
            
            if(itemA != itemB)
                Debug.Log("같은데이터지만 다른 아이템이다");
            int maxAmount = ciB.MaxAmount;
            int sum = ciA.Amount + ciB.Amount;
    
            if (sum <= maxAmount)
            {
                ciA.SetAmount(0);
                ciB.SetAmount(sum);
            }
            else
            {
                ciA.SetAmount(sum - maxAmount);
                ciB.SetAmount(maxAmount);
            }
        }
    
        UpdateSlot(from);
        UpdateSlot(to);
    }
    
    public void UpdateSlot(int index)
    {
        Item item = _items[index];
    
        if (item != null)
        {
            inventoryUI.SetItemIcon(index, item.itemData.IconImage);
        
            if (item is CountableItem ci)
            {
                if (ci.IsEmpty)
                {
                    _items[index] = null;
                    RemoveIcon();
                    return;
                }
                else
                {
                    inventoryUI.UpdateItemAmount(index, ci.Amount);
                }
            }
            else
            {
                inventoryUI.UpdateItemAmount(index, 0);//도구류는 0으로 
            }
        }
        else
        {
            RemoveIcon();
        }
    
        
        void RemoveIcon()//해당 인덱스 자리의 아이템이 null이 되었을 때 IsUsing도 false로
        {
            inventoryUI.RemoveItem(index);
            inventoryUI.HideItemAmountText(index);
        }
    }

    public Item GetItem(int index)
    {
        if (!IsValidIndex(index))
        {
            return null;
        }
        return _items[index];
    }

    public int GetItemCount(int index)
    {
        if (!IsValidIndex(index))
            return 0;

        if (_items[index] is CountableItem citem)
            return citem.Amount;
        
        return 1;
    }

    public string GetItemName(int index)
    {
        if (!IsValidIndex(index))
            return "";

        return _items[index].itemData.Name;
    }
    
    
}
