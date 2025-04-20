
using UnityEngine;

[CreateAssetMenu(fileName = "PortionItemData", menuName = "SO/CountableItemData/PortionItemData", order = int.MaxValue)]
public class PortionItemData : ConsumeItemData
{
    //넣을게 없네
    
    public override Item CreateItem()
    {
        return new PortionItem(this);
    }
}
