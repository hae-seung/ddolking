using UnityEngine;

public class PortionObject : CountableObject<PortionItem>
{
    [SerializeField] private PortionItemData data;
    
    protected override void MakeItemInstance()//풀이 아닌 최초 생성된 경우
    {
        countableItem = new PortionItem(data);
        itemData = data;
    }
    
}