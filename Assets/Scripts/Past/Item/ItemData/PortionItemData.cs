
using UnityEngine;

[CreateAssetMenu(fileName = "PortionItemData", menuName = "SO/CountableItemData/PortionItemData", order = int.MaxValue)]
public class PortionItemData : ConsumeItemData
{
    //즉시 적용 및 영구적용. ex)체력, 기력
    
    //경험치 물약 대비
    public int ExperienceAmount;
    
    public override Item CreateItem()
    {
        return new PortionItem(this);
    }
}
