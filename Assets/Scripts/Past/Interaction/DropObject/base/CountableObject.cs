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

    protected override void OnEnable()
    {
        if (!isSpawned)  //최초로 오브젝트가 생성된 경우 Awake에서 객체를 생성했으므로 풀에서 나왔을 땐 한 번 더 생성은 불필요함
        {
            isSpawned = true;
            return;
        }
        
        MakeItemInstance();
        base.OnEnable();
    }
    

    protected override void CollectItem()
    {
        if (!ObjectPoolManager.Instance.IsPoolRegistered(itemData.ID))
        {
            ObjectPoolManager.Instance.RegisterPrefab(itemData.ID, dropObjectPrefab);
        }
        
        
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