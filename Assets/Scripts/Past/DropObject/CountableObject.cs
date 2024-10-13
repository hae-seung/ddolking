using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountableObject : DropObject
{
    [SerializeField] private CountableItemData data;
    private CountableItem countableItem;

    protected override void Awake()
    {
        base.Awake();
        countableItem = new CountableItem(data); // default amount = 1
    }

    protected override void AddItemToInventory()
    {
        // 인벤토리에 추가할 수 있는 양 계산
        int amountBeforePickup = countableItem.Amount; // 현재 바닥에 있는 아이템의 수량
        int amountLeftAfterAdd = Inventory.Instance.Add(countableItem, countableItem.Amount); // 추가 후 남은 양 계산

        // 인벤토리에 추가되지 않은 양이 있으면, 드랍된 아이템의 수량을 업데이트
        if (amountLeftAfterAdd > 0)
        {
            countableItem.SetAmount(amountLeftAfterAdd); // 남은 양을 바닥에 남겨둠
            Debug.Log($"인벤토리에 {amountBeforePickup - amountLeftAfterAdd}개 추가됨. 바닥에 남은 양: {amountLeftAfterAdd}");
        }
        else
        {
            // 모든 양이 인벤토리에 추가되었으므로 드랍된 아이템 제거
            DestroyDropObject();
        }
    }
}


