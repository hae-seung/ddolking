using UnityEngine;

public abstract class CountableObject<T> : DropObject where T : CountableItem
{
    protected T countableItem;

    protected override void Awake()
    {
        MakeItemInstance();
        base.Awake();
    }

    protected abstract void MakeItemInstance();

    protected override void CollectItem()
    {
        int amountBeforePickup = countableItem.Amount;
        int amountLeftAfterAdd = Inventory.Instance.Add(countableItem, countableItem.Amount);

        if (amountLeftAfterAdd > 0)
        {
            countableItem.SetAmount(amountLeftAfterAdd);
            Debug.Log($"인벤토리에 {amountBeforePickup - amountLeftAfterAdd}개 추가됨. 남은 양: {amountLeftAfterAdd}");
        }
        else
        {
            DestroyDropObject();
        }
    }
}