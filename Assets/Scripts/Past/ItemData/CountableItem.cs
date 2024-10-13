using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CountableItem : Item
{
    public int Amount { get; protected set; }//현재 아이템 개수
    public int MaxAmount => CountableData.MaxAmount;//이 아이템 최대 갯수 default 99
    public bool IsMax => Amount >= CountableData.MaxAmount;
    public bool IsEmpty => Amount <= 0;
    public CountableItemData CountableData { get; private set; }
    
    public CountableItem(CountableItemData data, int amount = 1) : base(data)
    {
        CountableData = data;
        SetAmount(amount);
    }

    public void SetAmount(int amount)
    {
        Amount = Mathf.Clamp(amount, 0, MaxAmount);
    }
    
    public int AddAmountAndGetExcess(int amount)//개수 추가 및 최대치 초과량 반환(초과량 없을 경우 0)
    {
        int nextAmount = Amount + amount;
        SetAmount(nextAmount);

        return (nextAmount > MaxAmount) ? (nextAmount - MaxAmount) : 0;
    }
}
