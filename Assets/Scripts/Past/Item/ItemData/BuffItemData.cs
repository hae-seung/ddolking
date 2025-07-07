using UnityEngine;

[CreateAssetMenu(fileName = "BuffItemData", menuName = "SO/CountableItemData/BuffItemData", order = int.MaxValue)]
public class BuffItemData : ConsumeItemData
{
    private int buffId;
    [SerializeField] private int buffDuration;

    public override Item CreateItem()
    {
        return new BuffItem(this);
    }

    public int BuffDuration => buffDuration;

    public int BuffId => buffId;

#if UNITY_EDITOR
    private void OnValidate()
    {
        buffId = ID; // itemId는 부모 클래스에 있다고 가정
    }
#endif
}