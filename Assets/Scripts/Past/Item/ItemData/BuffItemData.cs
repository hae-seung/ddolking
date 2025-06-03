using UnityEngine;

[CreateAssetMenu(fileName = "BuffItemData", menuName = "SO/CountableItemData/BuffItemData", order = int.MaxValue)]
public class BuffItemData : ConsumeItemData
{
    //즉시 적용 및 영구적용. ex)체력, 기력
    
    [SerializeField] private int buffDuration;
    
    public override Item CreateItem()
    {
        return new BuffItem(this);
    }

    public int BuffDuration => buffDuration;
}