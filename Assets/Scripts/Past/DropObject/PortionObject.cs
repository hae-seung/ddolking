using UnityEngine;

public class PortionObject : CountableObject<PortionItem>
{
    [SerializeField] private PortionItemData data;
    protected override void MakeItemInstance()
    {
        countableItem = new PortionItem(data);
        itemId = data.ID;
    }

    protected override void OnEnable()
    {
        if (IsFirstAwake)
        {
            IsFirstAwake = false;
            return;
        }

        countableItem = new PortionItem(data);
        base.OnEnable();
    }

    public override int GetItemId()
    {
        return data.ID;
    }
}